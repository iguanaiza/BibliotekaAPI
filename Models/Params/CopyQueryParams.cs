namespace BibliotekaAPI.Models.Params
{
    public class CopyQueryParams
    {
        public string? Signature { get; set; }
        public int? InventoryNum { get; set; }
        public bool? Available { get; set; }

        public string? BookTitle { get; set; }
        public string? AuthorName { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        private const int MaxPageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
