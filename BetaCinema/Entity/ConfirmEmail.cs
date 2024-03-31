namespace BetaCinema.Entity
{
    public class ConfirmEmail
    {
        private DateTime date = DateTime.Now;
        public int ConfirmEmailId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime RequiredTime
        {
            get => date;
            private set
            {
                value = date;
                ExpiredTime = RequiredTime.AddMinutes(1);
            }
        }
        public DateTime ExpiredTime { get;private set; }
        public string ConfirmCode { get; set; }
        public bool? IsConfirm { get; set; }
    }
}
