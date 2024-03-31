using BetaCinema.Entity;
using BetaCinema.Helper;
using BetaCinema.IService;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Security.Claims;

namespace BetaCinema.Service
{
    public class VnPayService : IVnPayService
    {
        private AppDbContext dbContext;
        private MailHelper mailHelper;
        public VnPayService()
        {
            dbContext = new AppDbContext();
            mailHelper = new MailHelper();
        }

        public string CreatePaymentUrl(IConfiguration _config,HttpContext context,int BillId)
        {
            var bill = dbContext.Bill.Find(BillId);
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (bill.TotalMoney * 100).ToString()); //Số tiền thanh toán. Số tiền không 
            //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
            //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
            //là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", bill.CreateTime.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + new Random().Next(1000, 100000));
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ 
                                                                                      //thống của merchant.Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không được
                                                                                      //trùng lặp trong ngày

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashScret"] );
            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections,IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue("AccId");
            var user = dbContext.User.Find(Int32.Parse(userId));
            var email = user.Email;
            var vnpay = new VnPayLibrary();
            foreach(var(key,value)in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount"))/100;
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, "WVDGLALDVTGOYPOCNQTCVQYIOJKKQBHY");
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }
            string s = $@"
        <html>
        <head>
            <style>
            </style>
        </head>
        <body>
            <div>
                <h1>Welcome to BetaCinema</h1>
                <p>Thank you for your payment. This is transaction information</p>
                <ul>
                    <li>Phuong thuc thanh toan : VnPay</li>
                    <li>{vnp_OrderInfo}</li>
                    <li>Tong so tien : {vnp_Amount}</li>
                </ul>
                <p>Best regards,</p>
                <p>Your Team</p>
            </div>
        </body>
        </html>";
            mailHelper.GuiMail(email,s);
            UpdatePoint(user, vnp_Amount);
            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
        private void UpdatePoint(User user , long amount)
        {
            if (amount <= 200000) user.Point += 1;
            if(amount >200000 && amount <=500000) user.Point += 2;
            if(amount > 500000) user.Point += 3;
            if (user.Point > 5 && user.Point <=10) user.RankCustomerId = 1;
            if (user.Point > 10 && user.Point <=15) user.RankCustomerId = 2;
            if (user.Point > 15) user.RankCustomerId = 3;
            dbContext.Update(user); dbContext.SaveChanges();
        }
    }
}
