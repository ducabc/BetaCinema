using BetaCinema.Entity;
using BetaCinema.IService;
using BetaCinema.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService user;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            user = new UserService();
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("DangKy")]
        public IActionResult DangKy([FromBody] User userN)
        {
            var res = user.DangKy(userN);
            return Ok(res);
        }
        [HttpPost("DangNhap")]
        public IActionResult DangNhap([FromQuery] string username, string password)
        {
            var res = user.DangNhap(username, password,this.configuration,HttpContext);
            
            return Ok(res);
        }
        [HttpPost("QuenMatKhau")]
        public IActionResult QuenMatKhau([FromBody] string username, string password)
        {
            var res = user.QuenMatKhau(username, password);
            return Ok(res);
        }
        [HttpPost("XacThuc")]
        public IActionResult XacThuc([FromBody]ConfirmEmail confirm)
        {
            var res = user.XacThuc(confirm);
            return Ok(res);
        }
        [HttpGet("HienTaiKhoan")]
        public IActionResult HienTaiKhoan([FromQuery]Panigation? p,int? userId)
        {
            var res = user.HienThiUser(p,userId);
            return Ok(res);
        }
        [HttpPut("DoiMatKhau"),Authorize]
        public IActionResult DoiMatKhau([FromQuery]string old, string newPW)
        {
            var res = user.DoiMatKhau(old,newPW,this.httpContextAccessor);
            return Ok(res);
        }
        [HttpPost("RenewToken")]
        public IActionResult RenewToken([FromBody]TokenModel token)
        {
            var res = user.RenewToken(token, this.configuration);
            return Ok(res);
        }
    }
}
