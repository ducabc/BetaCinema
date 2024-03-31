namespace BetaCinema.Entity
{
    public class Room
    {
        public int RoomId { get; set; }
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Schedule>? Schedules { get; set; }
        public IEnumerable<Seat>? Seats { get; set; }
    }
}
