using BibliotekaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Year { get; set; }

        public string Description { get; set; }

        public string Isbn { get; set; }
        public string PageCount { get; set; }
        public bool IsVisible { get; set; }

        public string BookAuthor { get; set; }

        public string BookPublisher { get; set; }

        public string BookSeries { get; set; }

        public string BookCategory { get; set; }

        public string BookType { get; set; }

        public List<string> BookGenres { get; set; }
    }
}
