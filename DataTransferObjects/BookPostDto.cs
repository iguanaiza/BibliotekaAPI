using BibliotekaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects
{
    public class BookPostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        public string PageCount { get; set; }
        public bool IsVisible { get; set; }

        [Required]
        public int BookAuthorId { get; set; }

        [Required]
        public int BookPublisherId { get; set; }

        [Required]
        public int BookSeriesId { get; set; }

        [Required]
        public int BookTypeId { get; set; }

        [Required]
        public int BookCategoryId { get; set; }
        public List<int> BookGenreIds { get; set; }
    }
}
