using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //do używania [Required] czyli walidacji danych

namespace BibliotekaAPI.Models
{
    //klasa dot. ksiazki i jej danych
    public class Book
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public string Title { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public string Description { get; set; } //krótki opis ksiazki

        [Required]
        public string Isbn { get; set; } //numer ISBN

        [Required]
        public string PageCount { get; set; } //ilość stron

        #region Odwołania do innych
        public int BookAuthorId { get; set; }
        [ForeignKey(nameof(BookAuthorId))]
        public BookAuthor BookAuthor { get; set; } //autor

        public int BookPublisherId { get; set; }
        [ForeignKey(nameof(BookPublisherId))]
        public BookPublisher BookPublisher { get; set; } //wydawca

        public int BookSeriesId { get; set; }
        [ForeignKey(nameof(BookSeriesId))]
        public BookSeries BookSeries { get; set; } //seria np. Harry Potter

        public int BookTypeId { get; set; }
        [ForeignKey(nameof(BookTypeId))]
        public BookType BookType { get; set; } //typ ksiazki, rodzaj np. powieść

        public int BookCategoryId { get; set; }
        [ForeignKey(nameof(BookCategoryId))]
        public BookCategory BookCategory { get; set; } //jedno z trzech: lektura, podręcznik, pozostałe

        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();//gatunek ksiazki np. fantasy - moze byc wiele
        public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();//lista kopii
        #endregion
    }
}