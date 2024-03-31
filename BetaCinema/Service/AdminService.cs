using BetaCinema.Entity;
using BetaCinema.EntityDTO;
using BetaCinema.IService;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.TeleTrust;
using System.Data.Common;
using System.IO;
using System.Xml.Linq;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Net.Mime.MediaTypeNames;

namespace BetaCinema.Service
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext dbContext;
        public AdminService()
        {
            dbContext = new AppDbContext();
        }

        public string AddCinema(Cinema cinema)
        {
            dbContext.Cinema.Add(cinema);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string AddFood(Food food)
        {
            dbContext.Food.Add(food);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string AddMovie(Movie movie)
        {
            if (!dbContext.MovieType.Any(x => x.MovieTypeId == movie.MovieTypeId)) return "Movie type not available";
            if (!dbContext.Rate.Any(x => x.RateId == movie.RateId)) return "Rate of movie not available";
            dbContext.Movie.Add(movie);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string AddRoom(Room room)
        {
            if (!dbContext.Cinema.Any(x => x.CinemaId == room.CinemaId)) return "Cinema not available";
            dbContext.Room.Add(room);
            dbContext.SaveChanges();
            return "Thanh Cong";
        }

        public string AddSchedule(Schedule schedule)
        {
            var list = dbContext.Schedule.Where(x => x.RoomId == schedule.RoomId && x.StartAt.Day == schedule.StartAt.Day).ToList();
            foreach (var item in list)
            {
                if (DateTime.Compare(schedule.StartAt, item.StartAt) == 0) return "this time not available";
                else if (DateTime.Compare(schedule.StartAt, item.StartAt) < 0)
                {
                    if (DateTime.Compare(schedule.EndAt, item.StartAt) > 0) return "this time not available";
                }
                else
                {
                    if (DateTime.Compare(schedule.StartAt, item.EndAt) > 0) return "this time not available";
                }
            }
            dbContext.Schedule.Add(schedule);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string AddSeat(Seat seat)
        {
            if (!dbContext.SeatStatuse.Any(x => x.SeatStatusId == seat.SeatStatusId)) return "SeatStatus not available";
            if (!dbContext.Room.Any(x => x.RoomId == seat.RoomId)) return "Room not available";
            if (!dbContext.SeatType.Any(x => x.SeatTypeId == seat.SeatTypeId)) return "SeatType not available";
            dbContext.Seat.Add(seat);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string DeleteCinema(int id)
        {
            Cinema cinema = dbContext.Cinema.FirstOrDefault(x => x.CinemaId == id);
            if (cinema == null) return "can not find cinema";
            dbContext.Remove(cinema);
            dbContext.SaveChanges();
            return " Thanh cong";
        }

        public string DeleteFood(int id)
        {
            Food food = dbContext.Food.FirstOrDefault(x => x.FoodId == id);
            if (food == null) return "can not find this food";
            dbContext.Remove(food);
            dbContext.SaveChanges();
            return " Thanh cong";
        }

        public string DeleteMovie(int id)
        {
            Movie movie = dbContext.Movie.FirstOrDefault(x => x.MovieId == id);
            if (movie == null) return "can not find this movie";
            dbContext.Remove(movie);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string DeleteRoom(int id)
        {
            Room room = dbContext.Room.FirstOrDefault(x => x.RoomId == id);
            if (room == null) return "can not find this room";
            dbContext.Remove(room);
            dbContext.SaveChanges();
            return " Thanh cong";
        }

        public string DeleteSchedule(int id)
        {
            Schedule sche = dbContext.Schedule.FirstOrDefault(x => x.ScheduleId == id);
            if (sche == null) return "can not find this schedule";
            dbContext.Remove(sche);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string DeleteSeat(int id)
        {
            Seat seat = dbContext.Seat.FirstOrDefault(x => x.SeatId == id);
            if (seat == null) return "can not find this seat";
            dbContext.Remove(seat);
            dbContext.SaveChanges();
            return " Thanh cong";
        }

        public string UpdateCinema(Cinema cinema)
        {
            Cinema cinemaO = dbContext.Cinema.FirstOrDefault(x => x.CinemaId == cinema.CinemaId);
            if (cinemaO == null) return "this cinema not available";
            cinemaO.Address = cinema.Address;
            cinemaO.Descriptiom = cinema.Descriptiom;
            cinemaO.Code = cinema.Code;
            cinemaO.NameOfCinema = cinema.NameOfCinema;
            cinemaO.IsActive = cinema.IsActive;
            dbContext.Cinema.Update(cinemaO);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string UpdateFood(Food food)
        {
            Food OFood = dbContext.Food.FirstOrDefault(x => x.FoodId == food.FoodId);
            if (OFood == null) return "this food not available";
            OFood.Price = food.Price;
            OFood.Description = food.Description;
            OFood.Image = food.Image;
            OFood.NameOfFood = food.NameOfFood;
            OFood.IsActive = food.IsActive;
            dbContext.Food.Update(OFood); dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string UpdateMovie(Movie movie)
        {
            Movie OMovie = dbContext.Movie.FirstOrDefault(x => x.MovieId == movie.MovieId);
            if (OMovie == null) return "this movie not available";
            OMovie.MovieDuration = movie.MovieDuration;
            OMovie.EndTime = movie.EndTime;
            OMovie.PremiereDate = movie.PremiereDate;
            OMovie.Description = movie.Description;
            OMovie.Director = movie.Director;
            OMovie.Image = movie.Image;
            OMovie.HeroImage = movie.HeroImage;
            OMovie.Language = movie.Language;
            OMovie.MovieTypeId = movie.MovieTypeId;
            OMovie.Name = movie.Name;
            OMovie.RateId = movie.RateId;
            OMovie.Trailer = movie.Trailer;
            OMovie.IsActive = movie.IsActive;
            if (!dbContext.MovieType.Any(x => x.MovieTypeId == OMovie.MovieTypeId)) return "Movie type not available";
            if (!dbContext.Rate.Any(x => x.RateId == OMovie.RateId)) return "Rate of movie not available";
            dbContext.Update(OMovie);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string UpdateRoom(Room room)
        {
            Room ORoom = dbContext.Room.FirstOrDefault(x => x.RoomId == room.RoomId);
            if (ORoom == null) return "this room not available";
            ORoom.Capacity = room.Capacity;
            ORoom.Type = room.Type;
            ORoom.Description = room.Description;
            ORoom.CinemaId = room.CinemaId;
            ORoom.Code = room.Code;
            ORoom.Name = room.Name;
            ORoom.IsActive = room.IsActive;
            if (!dbContext.Room.Any(x => x.CinemaId == ORoom.CinemaId)) return "Cinema not available";
            dbContext.Update(ORoom); dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string UpdateSchedule(Schedule schedule)
        {
            Schedule sche = dbContext.Schedule.FirstOrDefault(x => x.ScheduleId == schedule.ScheduleId);
            if (sche == null) return "this schedule not available";
            var list = dbContext.Schedule.Where(x => x.RoomId == schedule.RoomId && x.StartAt.Day == schedule.StartAt.Day).ToList();
            foreach (var item in list)
            {
                if (DateTime.Compare(schedule.StartAt, item.StartAt) == 0) return "this time not available";
                else if (DateTime.Compare(schedule.StartAt, item.StartAt) < 0)
                {
                    if (DateTime.Compare(schedule.EndAt, item.StartAt) > 0) return "this time not available";
                }
                else
                {
                    if (DateTime.Compare(schedule.StartAt, item.EndAt) > 0) return "this time not available";
                }
            }
            sche.Price = schedule.Price;
            sche.StartAt = schedule.StartAt;
            sche.EndAt = schedule.EndAt;
            sche.Code = schedule.Code;
            sche.MovieId = schedule.MovieId;
            sche.Name = schedule.Name;
            sche.RoomId = schedule.RoomId;
            sche.IsActive = schedule.IsActive;
            dbContext.Update(sche);
            dbContext.SaveChanges();
            return "Thanh cong";
        }

        public string UpdateSeat(Seat seat)
        {
            Seat OSeat = dbContext.Seat.FirstOrDefault(x => x.SeatId == seat.SeatId);
            if (OSeat == null) return "this seat not available";
            OSeat.Number = seat.Number;
            OSeat.SeatStatusId = seat.SeatStatusId;
            OSeat.Line = seat.Line;
            OSeat.RoomId = seat.RoomId;
            OSeat.IsActive = seat.IsActive;
            OSeat.SeatTypeId = seat.SeatTypeId;
            if (!dbContext.SeatStatuse.Any(x => x.SeatStatusId == OSeat.SeatStatusId)) return "SeatStatus not available";
            if (!dbContext.Room.Any(x => x.RoomId == OSeat.RoomId)) return "Room not available";
            if (!dbContext.SeatType.Any(x => x.SeatTypeId == OSeat.SeatTypeId)) return "SeatType not available";
            dbContext.Update(OSeat); dbContext.SaveChanges();
            return "Thanh cong";
        }

        //thong ke doanh so cua rap
        public List<CinemaDTO> DoanhSo(DateTime before, DateTime then)
        {
            var x = dbContext.Bill.Select(x => new
            {
                Money = x.TotalMoney,
                date = x.UpdateTime,
                CinemaId = x.BillTickets.First().Ticket.Schedule.Room != null ? x.BillTickets.First().Ticket.Schedule.Room.CinemaId : 0,
            }).ToList();
            var listCinema = x.Where(y => y.CinemaId != 0 && y.date > before && y.date < then).GroupBy(y => y.CinemaId).Select(g => new CinemaDTO
            {
                CinemaId = g.Key,
                NameOfCinema = dbContext.Cinema.Find(g.Key).NameOfCinema,
                Address = dbContext.Cinema.Find(g.Key).Address,
                Descriptiom = dbContext.Cinema.Find(g.Key).Descriptiom,
                DoanhThu = g.Sum(y => y.Money)
            }).ToList();
            return listCinema;
        }
        //thong ke mon an ban chay nhat
        public List<FoodDTO> ListFoodHot()
        {
            var food = dbContext.Food.Select(g => new FoodDTO
            {
                NameOfFood = g.NameOfFood,
                Price = g.Price,
                Description = g.Description,
                Image = g.Image,
                CountBuy = g.BillFoods.Where(y => y.Bill.UpdateTime > DateTime.Now.AddDays(-7)).Sum(y => y.Quantity)
            })
            .Where(x => x.CountBuy != 0).OrderByDescending(x => x.CountBuy).ToList();
            return food;
        }
        //quan ly thong tin nguoi dung
        public List<User> GetUser()
        {
            var listU = dbContext.User.Include(x => x.RankCustomer).ToList();
            return listU;
        }
        public User UpdateUser(User user)
        {
            User OUser = dbContext.User.Find(user.UserId);
            if (OUser == null) { return null; }
            OUser.Point = user.Point;
            OUser.UserName = user.UserName;
            OUser.Email = user.Email;
            OUser.Name = user.Name;
            OUser.PhoneNumber = user.PhoneNumber;
            OUser.Password = user.Password;
            OUser.RankCustomerId = user.RankCustomerId;
            OUser.UserStatusId = user.UserStatusId;
            OUser.IsActive = user.IsActive;
            OUser.RoleId = user.RoleId;
            dbContext.Update(OUser); dbContext.SaveChanges();
            return OUser;
        }
        public string DeleteUser(int id)
        {
            var user = dbContext.User.Find(id);
            if (user == null) { return "user not aavailable"; }
            dbContext.Remove(user); dbContext.SaveChanges();
            return "Thanh cong";
        }
    }
}
