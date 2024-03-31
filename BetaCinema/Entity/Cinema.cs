namespace BetaCinema.Entity
{
    public class Cinema
    {
        public int CinemaId { get; set; }
        public string Address { get; set; }
        public string Descriptiom { get; set; }
        public string Code { get; set; }
        public string NameOfCinema { get; set; }
        public bool? IsActive { get; set; }
        public IEnumerable<Room>? Rooms { get; set; }
    }
    public class CinemaDTO
    {
        public string NameOfCinema { get; set; }
        public int CinemaId { get; set; }
        public string Address { get; set; }
        public string Descriptiom { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public double? DoanhThu {  get; set; }
    }
}
