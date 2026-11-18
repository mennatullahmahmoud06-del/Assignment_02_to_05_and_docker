namespace Assignment.Results
{
    public class PaginatedList<T>
    {
        // Attributes
        private const int _maxPageSize = 20;
        private int _pageSize = 10;

        // Properties
        public int PageNumber { get; set; } = 1;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
        public IEnumerable<T>? Data { get; set; }
    }
}
