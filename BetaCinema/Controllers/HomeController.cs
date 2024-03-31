using BetaCinema.Entity;
using BetaCinema.Helper;
using BetaCinema.IService;
using BetaCinema.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IHomeService homeService;
        private readonly IVnPayService vnPayService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor,IConfiguration config)
        {
            homeService = new HomeService();
            vnPayService = new VnPayService();
            this.httpContextAccessor = httpContextAccessor;
            this._config = config;
        }

        [HttpGet("HotMovie")]
        public IActionResult HotMovie()
        {
            var res = homeService.HotMovie();
            return Ok(res);
        }
        [HttpGet("SelectCinemaMovie")]
        public IActionResult SelectCinema([FromQuery]int c)
        {
            var res = homeService.ListMovieOfCinema(c);
            return Ok(res);
        }
        [HttpGet("SelectRoomMovie")]
        public IActionResult SelectRoomMovie([FromQuery] int r)
        {
            var res = homeService.ListMovieOfRoom(r);
            return Ok(res);
        }
        [HttpGet("SelectSeatInRoom")]
        public IActionResult SelectSeatInRoom([FromQuery] int r)
        {
            var res = homeService.ListSeatInRoom(r);
            return Ok(res);
        }
        [HttpGet("SelectCinemaOfMovie")]
        public IActionResult SelectCinemaOfMovie([FromQuery] int r)
        {
            var res = homeService.ListCinemaOfMovie(r);
            return Ok(res);
        }
        [HttpPost("CreateBillFood"),Authorize]
        public IActionResult CreateBillFood([FromBody]BillFood billFood)
        {
            var res = homeService.CreateBillFood(billFood);
            return Ok(res);
        }
        [HttpPost("CreateBillTicket"), Authorize]
        public IActionResult CreateBillTicket([FromBody] List<int> seatId, int scheduleId)
        {
            var res = homeService.CreateTicket(this.httpContextAccessor,seatId,scheduleId);
            return Ok(res);
        }
        [HttpPut("CreateBill"), Authorize]
        public IActionResult CreateBill([FromQuery]int billId)
        {
            var res = homeService.PayPalBill(this.httpContextAccessor,billId);
            return Ok(res);
        }

        [HttpPost("ThanhToan"), Authorize]
        public IActionResult ThanhToan([FromQuery]int billId)
        {
            var res = vnPayService.CreatePaymentUrl(_config,HttpContext,billId);
            return Ok(res);
;       }

        [HttpGet("PaymentBack"),Authorize]
        public IActionResult PaymentBack()
        {
            var respond = vnPayService.PaymentExecute(Request.Query,this.httpContextAccessor);
            if (respond.VnPayResponseCode !="00")
            {
                return null;
            }
            
            return Ok(respond);
        }
    }
}
