using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Data
{
    public class PagedList<T>
    {
        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        private PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize, CancellationToken cancellation)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var totalCount = await query.CountAsync(cancellation);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellation);

            return new(items, page, pageSize, totalCount);
        }
    }
}
