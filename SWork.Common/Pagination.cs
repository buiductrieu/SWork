

namespace SWork.Common
{
    public class Pagination<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public Pagination(ICollection<T> items, int totalItemsCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public Pagination() { }

        public int TotalPagesCount
        {
            get
            {
                var temp = TotalItemsCount / PageSize;
                if (TotalItemsCount % PageSize == 0)
                {
                    return temp;
                }
                return temp + 1;
            }
        }

        public bool Next => PageIndex + 1 < TotalPagesCount;
        public bool Previous => PageIndex > 1;
        public ICollection<T> Items { get; set; }
    }
}
