namespace BetaCinema.Entity
{
    public class PageResult<T>
    {
        public PageResult(Panigation panigation, IEnumerable<T> data)
        {
            this.panigation = panigation;
            Data = data;
        }

        public Panigation panigation { get; set; }
        public IEnumerable<T> Data { get; set; }
        public static IEnumerable<T> ToPageResult(Panigation panigation, IEnumerable<T> Data)
        {
            panigation.PageNumber = panigation.PageNumber < 1 ? 1 : panigation.PageNumber;
            panigation.PageSize = panigation.PageSize == -1 ? Data.Count() : panigation.PageSize;
            panigation.TotalRecord = Data.Count();
            var result = Data.Skip(panigation.PageSize * (panigation.PageNumber - 1)).Take(panigation.PageSize).AsQueryable();
            return result;
        }

    }
}
