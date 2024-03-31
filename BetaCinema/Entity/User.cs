using System.ComponentModel.DataAnnotations.Schema;

namespace BetaCinema.Entity
{
    public class User
    {
        private int? usStatus =1;
        public int UserId { get; set; }
        public int Point { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int? RankCustomerId { get; set; }
        public  RankCustomer? RankCustomer { get; set; }
        public int? UserStatusId 
        {
            get { return usStatus; }
            set
            {
                usStatus = value;
                if (UserStatusId == 1) IsActive = false;
                if (UserStatusId == 2) IsActive = true;
            }
        }
        public UserStatus? UserStatus { get; set; }
        public bool IsActive { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public IEnumerable<ConfirmEmail>? ConfirmEmails { get; set; }
        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
        [ForeignKey("CustomerId")]
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
