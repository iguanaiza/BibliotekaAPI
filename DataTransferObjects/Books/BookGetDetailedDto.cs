using BibliotekaAPI.DataTransferObjects.BookCopies;

namespace BibliotekaAPI.DataTransferObjects.Books
{
    public class BookGetDetailedDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Description { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public int PageCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVisible { get; set; }
        public string? ImageUrl { get; set; }
        public string? Subject { get; set; }
        public string? Class { get; set; }

        public string BookAuthor { get; set; } = null!;

        public string BookPublisher { get; set; } = null!;

        public string? BookSeries { get; set; }

        public string BookCategory { get; set; } = null!;

        public string BookType { get; set; } = null!;

        public List<string> BookGenres { get; set; } = new();

        public List<string>? BookSpecialTags { get; set; }

        public List<CopyGetDetailedDto>? BookCopies { get; set; }
        public int CopyCount { get; set; }
        public int AvailableCopyCount { get; set; }
    }
}
