namespace BetaCinema.Entity
{
    public class SeatType
    {
        public int SeatTypeId { get; set; }
        public string NameType { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
