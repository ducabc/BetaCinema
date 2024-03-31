namespace BetaCinema.Entity
{
    public class SeatStatus
    {
        public int SeatStatusId { get; set; }
        public string Code { get; set; }
        public string NameStatus { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
