namespace CursoOnline.Web.Util
{
    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        private PagedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);

        public static PagedList<T> Create(IEnumerable<T> source, HttpRequest request)
        {
            const int pageSize = 10;
            int.TryParse(request.Query["page"], out int pageIndex);
            pageIndex = pageIndex > 0 ? pageIndex : 1;

            var enumerable = source == null ? new List<T>() : source.ToList();
            var count = enumerable.Count();
            var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
