using BetaCinema.Entity;
using BetaCinema.EntityDTO;

namespace BetaCinema.IService
{
    public interface IHomeService
    {
        public IEnumerable<MovieDTO> HotMovie();
        public IEnumerable<MovieDTO> ListMovieOfCinema(int CinemaId);
        public IEnumerable<MovieDTO> ListMovieOfRoom(int r);
        public IEnumerable<SeatDTO> ListSeatInRoom(int schedule);
        public IEnumerable<CinemaDTO> ListCinemaOfMovie(int movieId);
        public List<BillTicket> CreateTicket(IHttpContextAccessor httpContext, List<int> seatId, int scheduleId);
        public BillFood CreateBillFood(BillFood billFood);
        public Bill PayPalBill(IHttpContextAccessor httpContext,int billId);
    }
}
