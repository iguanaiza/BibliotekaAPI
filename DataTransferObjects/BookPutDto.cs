using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects
{
    public class BookPutDto
    {
        public string? Title { get; set; }

        public string? Year { get; set; }

        public string? Description { get; set; }

        public string? Isbn { get; set; }

        public string? PageCount { get; set; }
        public bool? IsVisible { get; set; }

        public int? BookAuthorId { get; set; }

        public int? BookPublisherId { get; set; }

        public int? BookSeriesId { get; set; }

        public int? BookTypeId { get; set; }

        public int? BookCategoryId { get; set; }
        public List<int>? BookGenreIds { get; set; }
    }
}
