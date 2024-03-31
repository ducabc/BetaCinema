namespace BetaCinema.Entity
{
    public class MovieType
    {
        public int MovieTypeId { get; set; }
        public string MovieTypeName { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
