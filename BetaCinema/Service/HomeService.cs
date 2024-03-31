using BetaCinema.Helper;
using BetaCinema.Entity;
using BetaCinema.IService;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BetaCinema.EntityDTO;

namespace BetaCinema.Service
{
    public class HomeService : IHomeService
    {
        private readonly AppDbContext dbContext;
        public HomeService()
        {
            dbContext = new AppDbContext();
        }

        public IEnumerable<MovieDTO> HotMovie()
        {
            var movie = dbContext.Movie.Include(x => x.Schedule).ThenInclude(x => x.Tickets).ToList();
            var list = MovieMapper.Mapper(movie);
            var result = list.OrderByDescending(x => x.View);
            return result;
        }
        public IEnumerable<MovieDTO> ListMovieOfCinema(int c)
        {
            var list = dbContext.Movie.Include(x => x.Schedule).ThenInclude(x => x.Room)
                .Include(x => x.Schedule).ThenInclude(x => x.Tickets).Where(x => x.Schedule.Any(y => y.Room.CinemaId == c)).ToList();
            var result = MovieMapper.Mapper(list);
            return result;
        }
        public IEnumerable<MovieDTO> ListMovieOfRoom(int r)
        {
            var list = dbContext.Movie.Include(x => x.Schedule).ThenInclude(x => x.Room)
               .Include(x => x.Schedule).ThenInclude(x => x.Tickets).Where(x => x.Schedule.Any(y => y.RoomId == r)).ToList();
            var result = MovieMapper.Mapper(list);
            return result;
        }
        //danh sach ghe trong phong theo suat chieu da chon
        public IEnumerable<SeatDTO> ListSeatInRoom(int ScheduleId)
        {
            var schedule = dbContext.Schedule.Find(ScheduleId);
            var seat = dbContext.Seat.Include(x => x.Room).Include(x => x.SeatStatus).Include(x => x.SeatType).Where(x => x.RoomId == schedule.RoomId).ToList();
            var ticket = dbContext.Ticket.Where(x => x.ScheduleId == schedule.ScheduleId).AsQueryable();
            var result = MovieMapper.Mapper(seat);
            foreach (var i in result)
            {
                if (ticket.Any(x => x.SeatId == i.SeatId)) i.SeatStatus = "da dat";
            }
            return result;
        }

        //luong xu ly dat ve

        //danh sach rap chieu phim duoc chon
        public IEnumerable<CinemaDTO> ListCinemaOfMovie(int movieId)
        {
            var cinema = dbContext.Cinema.Include(x => x.Rooms).ThenInclude(x => x.Schedules).
                Where(x => x.Rooms.Any(y => y.Schedules.Any(z => z.MovieId == movieId))).ToList();
            return MovieMapper.Mapper(cinema);
        }
        //danh sach phong trong rap duoc chon
        public IEnumerable<Room> ListRoomOfCinema(int cinemaId)
        {
            var result = dbContext.Room.Where(x => x.CinemaId == cinemaId).AsQueryable();
            return result;
        }
        //danh sach suat chieu theo phong
        public IEnumerable<Schedule> ListScheduleOfRoom(int roomId, int movieId)
        {
            var result = dbContext.Schedule.Where(x => x.RoomId == roomId && x.MovieId == movieId).AsQueryable();
            return result;
        }
        //tao ve theo suat chieu va ghe da chon
        public List<BillTicket> CreateTicket(IHttpContextAccessor httpContext, List<int> seatId, int scheduleId)
        {
            var bill = CreateBill(httpContext);
            dbContext.Bill.Add(bill);
            dbContext.SaveChanges();
            var listTicket = dbContext.Ticket.Where(x => x.ScheduleId == scheduleId).ToList();
            var list = new List<BillTicket>();
            foreach(var i in seatId)
            {
                if (listTicket.Any(x => x.SeatId == i)) continue;
                Ticket ticket = new Ticket()
                {
                    Code = "abc",
                    ScheduleId = scheduleId,
                    SeatId = i,
                    PriceTicket = dbContext.Schedule.FirstOrDefault(x => x.ScheduleId == scheduleId).Price,
                    IsActive = true,
                };
                dbContext.Ticket.Add(ticket);
                dbContext.SaveChanges();
                BillTicket billTicket = new BillTicket()
                {
                    Quantity = 1,
                    BillId = bill.BillId,
                    TicketId = ticket.TicketId,
                };
                dbContext.BillTicket.Add(billTicket);
                dbContext.SaveChanges();
                list.Add(billTicket);
            }
            return list;
        }
        //chon do an
        public IEnumerable<Food> ListFood(int foodId)
        {
            var result = dbContext.Food.AsQueryable();
            return result;
        }
        public BillFood CreateBillFood(BillFood billFood)
        {
            dbContext.BillFood.Add(billFood);
            dbContext.SaveChanges();
            return billFood;
        }


        //tinh tien
        public Bill PayPalBill(IHttpContextAccessor httpContext,int billId)
        {
            var bill = dbContext.Bill.Find(billId);
            bill.UpdateTime = DateTime.Now;
            var promotion = dbContext.Promotion.Where(x => x.StartTime < DateTime.Now && x.EndTime > DateTime.Now).FirstOrDefault(x => x.PromotionId == bill.PromotionId);
            var percent = promotion != null ? promotion.Percent : 0;
            bill.TotalMoney = dbContext.BillFood.Include(x => x.Food).Sum(x => x.Quantity * (int)x.Food.Price) 
                + dbContext.BillTicket.Include(x=>x.Ticket).Sum(x=>x.Quantity * x.Ticket.PriceTicket) * (100 - percent)/100;
            dbContext.Bill.Update(bill); dbContext.SaveChanges();
            return bill;
        }
        public Bill CreateBill(IHttpContextAccessor httpContext)
        {
            var id = GetMyName(httpContext);
            User user = dbContext.User.Find(Int32.Parse(id));
            var bill = new Bill()
            {
                TradingCode = "APTX",
                CreateTime = DateTime.Now,
                CustomerId = user.UserId,
                Name = "khach dat ve",
                UpdateTime = DateTime.Now,
                PromotionId = dbContext.Promotion.FirstOrDefault(x=>x.RankCustomerId == user.RankCustomerId).PromotionId,
                BillStatusId = 1,
                Title = "ve thuong",
                TotalMoney = 0
            };
            return bill;
        }
        private string GetMyName(IHttpContextAccessor httpContextAccessor)
        {
            var result = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.User.FindFirstValue("AccId");
            }
            return result;
        }
    }
}
