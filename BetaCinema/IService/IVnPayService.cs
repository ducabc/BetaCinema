using BetaCinema.Entity;

namespace BetaCinema.IService
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(IConfiguration _config, HttpContext context, int BillId);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections,IHttpContextAccessor httpContextAccessor);
    }
}
