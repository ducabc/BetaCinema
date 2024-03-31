using BetaCinema.Entity;
using BetaCinema.EntityDTO;

namespace BetaCinema.IService
{
    public interface IAdminService
    {
        public string AddCinema(Cinema cinema);
        public string UpdateCinema(Cinema cinema);
        public string DeleteCinema(int id);

        public string AddRoom(Room room);
        public string UpdateRoom(Room room);
        public string DeleteRoom(int id);

        public string AddSeat(Seat seat);
        public string UpdateSeat(Seat seat);
        public string DeleteSeat(int id);

        public string AddFood(Food food);
        public string UpdateFood(Food food);
        public string DeleteFood(int id);

        public string AddMovie(Movie movie);
        public string UpdateMovie(Movie movie);
        public string DeleteMovie(int id);

        public string AddSchedule(Schedule schedule);
        public string UpdateSchedule(Schedule schedule);

        public string DeleteSchedule(int id);

        public List<CinemaDTO> DoanhSo(DateTime before, DateTime then);
        public List<FoodDTO> ListFoodHot();
    }
}
