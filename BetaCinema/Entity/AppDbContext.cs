using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Entity
{
    public class AppDbContext : DbContext
    {
        public DbSet<Bill> Bill { get; set; }
        public DbSet<BillFood> BillFood { get; set; }
        public DbSet<BillStatus> BillStatus { get; set; }
        public DbSet<BillTicket> BillTicket { get; set; }
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<ConfirmEmail> ConfirmEmail { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieType> MovieType { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<RankCustomer> RankCustomer { get; set; }
        public DbSet<Rate> Rate { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<SeatStatus> SeatStatuse { get; set; }
        public DbSet<SeatType> SeatType { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserStatus> UserStatuse { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-75JL995\ASPNET; Database = BetaCinema; Trusted_Connection = True;");
        }
    }
}
