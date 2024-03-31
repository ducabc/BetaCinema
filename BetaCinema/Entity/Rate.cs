namespace BetaCinema.Entity
{
    public class Rate
    {
        public int RateId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
