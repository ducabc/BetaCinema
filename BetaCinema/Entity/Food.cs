namespace BetaCinema.Entity
{
    public class Food
    {
        public int FoodId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string NameOfFood { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<BillFood>? BillFoods { get; set; }
    }
}
