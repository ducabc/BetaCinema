namespace BetaCinema.Entity
{
    public class Panigation
    {
        public Panigation()
        {
            PageSize = -1;
            PageNumber = 1;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecord { get; set; }
        public int ToatalPage
        {
            get
            {
                if (PageSize == 0) return 0;
                var total = TotalRecord / PageSize;
                if (TotalRecord % PageSize != 0) total++;
                return total;
            }
        }
    }
}
