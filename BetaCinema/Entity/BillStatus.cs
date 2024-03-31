namespace BetaCinema.Entity
{
    public class BillStatus
    {
        public int BillStatusId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Bill> Bills { get; set; }
    }
}
