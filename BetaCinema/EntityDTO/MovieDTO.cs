using BetaCinema.Entity;

namespace BetaCinema.EntityDTO
{
    public class MovieDTO
    {
        public int MovieId { get; set; }
        public int MovieDuration { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public int RateId { get; set; }
        public string Trailer { get; set; }
        public int? View { get; set; }
        public IEnumerable<Schedule>? Schedule { get; set; }

    }
}
