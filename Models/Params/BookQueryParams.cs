namespace BibliotekaAPI.Models.Params
{
    public class BookQueryParams
    {
        public string? Title { get; set; }
        public int? Year { get; set; } 
        public string? Isbn { get; set; }
        public bool? IsVisible { get; set; }
        public string? BookAuthor { get; set; }
        public string? BookPublisher { get; set; }
        public string? BookSeries { get; set; }
        public string? BookCategory { get; set; }
        public string? BookType { get; set; }
        public List<string>? BookSpecialTags { get; set; }
        public List<string>? BookGenres { get; set; } 
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
