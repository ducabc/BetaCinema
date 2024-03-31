using BetaCinema.Entity;
using BetaCinema.Helper;
using BetaCinema.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BetaCinema.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext dbContext;
        private MailHelper mailHelper;
        Random rd = new Random();
        public UserService()
        {
            dbContext = new AppDbContext();
            mailHelper = new MailHelper();
        }

        public string DangKy(User user)
        {
            var listU = dbContext.User.AsQueryable();
            if (listU.Any(x => x.UserName == user.UserName)) return null;
            string s = rd.Next(0, 1000000).ToString();
            ConfirmEmail confirm = new ConfirmEmail()
            {
                UserId = user.UserId,
                User = user,
                ConfirmCode = s,
                IsConfirm = false
            };
            dbContext.ConfirmEmail.Add(confirm);
            dbContext.User.Add(user);
            mailHelper.GuiMail(user.Email, s);
            dbContext.SaveChanges();
            return "Thanh cong";
        }
        //private void GuiMail(string emailVerify,string random)
        //{
        //    var email = new MimeMessage();

        //    email.From.Add(new MailboxAddress("Sender Name", "pthanhduc3012@gmail.com"));
        //    email.To.Add(new MailboxAddress("Receiver Name", emailVerify));

        //    email.Subject = "Verify Account";
        //    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = random
        //    };

        //    using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        //    {
        //        smtp.Connect("smtp.gmail.com", 587, false);

        //        // Note: only needed if the SMTP server requires authentication
        //        smtp.Authenticate("pthanhduc3012@gmail.com", "yjef ziit qnye vyqz");

        //        smtp.Send(email);
        //        smtp.Disconnect(true);
        //    }
        //}
        
        public PageResult<User> HienThiUser(Panigation? panigation, int? UserId)
        {
            var ListU = dbContext.User.AsQueryable();
            if(UserId.HasValue) ListU = ListU.Where(x=>x.UserId ==  UserId);
            var result = PageResult<User>.ToPageResult(panigation, ListU);
            return new PageResult<User>(panigation, result);
        }

        public string QuenMatKhau(string userName,string password)
        {
            User user = dbContext.User.Single(x=>x.UserName == userName);
            if (user == null) return "khong tim thay ten dang nhap nay";
            user.Password = password;
            dbContext.Update(user);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string XacThuc(ConfirmEmail confirm)
        {
            var listE = dbContext.ConfirmEmail.Where(x=>x.UserId == confirm.UserId);
            User user = dbContext.User.Single(x => x.UserId == confirm.UserId);
            if (user == null) return "Tai khoan chua ton tai";
            if (!listE.Any(x => x.ConfirmCode == confirm.ConfirmCode)) return "Ma xac thuc khong dung";
            ConfirmEmail cfo = dbContext.ConfirmEmail.FirstOrDefault(x => x.ConfirmCode == confirm.ConfirmCode && x.UserId == confirm.UserId);
            cfo.IsConfirm = true; dbContext.Update(cfo);
            user.UserStatusId = 2;
            dbContext.Update(user);
            dbContext.SaveChanges();
            return "Xac thuc thanh cong";
        }
        public string DoiMatKhau(string oldpassword, string newpassword,IHttpContextAccessor httpContext)
        {
            var id = GetMyName(httpContext);
            User user = dbContext.User.Find(Int32.Parse(id));
            if (user.Password != oldpassword) return "mat khau cu chua dung";
            user.Password = newpassword;
            dbContext.Update(user);
            dbContext.SaveChanges();
            return "Doi thanh cong";
        }
        private string GetMyName(IHttpContextAccessor httpContextAccessor)
        {
            var result = string.Empty;
            if(httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.User.FindFirstValue("AccId");
            }
            return result;
        }

        public TokenModel DangNhap(string username, string password,IConfiguration configuration,HttpContext httpContext)
        {
            User user = dbContext.User.Include(x=>x.Role).FirstOrDefault(x => x.UserName == username && x.Password == password);
            if (user == null) return null;
            user.IsActive = true; dbContext.Update(user); dbContext.SaveChanges();
            var token = CreateToken(user,configuration);
            return token;
        }

        private TokenModel CreateToken(User user, IConfiguration configuration)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("AccId",user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.RoleName)
            }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var jwt = new JwtSecurityTokenHandler().CreateToken(token);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshTo = GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = refreshTo,
                UserId = user.UserId,
                ExpiredTime = DateTime.UtcNow.AddMinutes(1)
            };
            dbContext.RefreshToken.Add(refreshToken);
            dbContext.SaveChanges();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshTo
            };
        }
        public TokenModel RenewToken(TokenModel tokenModel,IConfiguration configuration)
        {
            var jwt = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value);
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false,
            };
            try
            {
                var tokenInverification = jwt.ValidateToken(tokenModel.AccessToken, tokenValidateParam, out var validatedToken);
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return null;
                    }
                }
                var utcExprireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = convertUnixTimeToDateTime(utcExprireDate);
                if(expireDate > DateTime.UtcNow)
                {
                    return null;
                }
                var storedToken = dbContext.RefreshToken.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
                if (storedToken == null) return null;

                var user = dbContext.User.Include(x=>x.Role).SingleOrDefault(x=>x.UserId == storedToken.UserId);
                var token = CreateToken(user,configuration);
                return token;
            }
            catch (Exception ex)
            {
                return null;
            };
        }

        private DateTime convertUnixTimeToDateTime(long utcExprireDate)
        {
            var datetimrInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            datetimrInterval.AddSeconds(utcExprireDate).ToUniversalTime();

            return datetimrInterval;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
