namespace BetaCinema.Entity
{
    public class UserStatus
    {
        public int UserStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
