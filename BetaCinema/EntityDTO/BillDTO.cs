using BetaCinema.Entity;

namespace BetaCinema.EntityDTO
{
    public class BillDTO
    {
        public int BillId { get; set; }
        public double TotalMoney { get; set; }
        public int CinemaId {  get; set; }
        public IEnumerable<BillFood>? BillFoods { get; set; }
        public IEnumerable<BillTicket>? BillTickets { get; set; }
    }
}
