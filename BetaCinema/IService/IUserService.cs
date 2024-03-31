using BetaCinema.Entity;

namespace BetaCinema.IService
{
    public interface IUserService
    {
        public TokenModel DangNhap(string username, string password, IConfiguration configuration,HttpContext httpContext);
        public string DangKy(User user);
        public string XacThuc(ConfirmEmail confirm);
        public string QuenMatKhau(string username,string password);
        public PageResult<User> HienThiUser(Panigation? panigation, int? UserId);
        public string DoiMatKhau(string oldpassword, string newpassword, IHttpContextAccessor httpContext);
        public TokenModel RenewToken(TokenModel tokenModel, IConfiguration configuration);
    }
}
