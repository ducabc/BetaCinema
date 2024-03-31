using static System.Net.Mime.MediaTypeNames;

namespace BetaCinema.EntityDTO
{
    public class FoodDTO
    {
        public string NameOfFood { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int CountBuy { get; set; }
    }
}
