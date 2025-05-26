using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //do używania [Required] czyli walidacji danych

namespace BibliotekaAPI.Models
{
    //klasa dot. ksiazki i jej danych
    public class Book
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public string Title { get; set; }

        public string Year { get; set; }

        public string Description { get; set; } //krótki opis ksiazki

        public string Isbn { get; set; } //numer ISBN

        public string PageCount { get; set; } //ilość stron

        public bool IsDeleted { get; set; } = false;//do soft delete
        public bool IsVisible { get; set; } = true;//do widoku w katalogu

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

        public ICollection<BookBookGenre> BookBookGenres { get; set; }//gatunek ksiazki np. fantasy - moze byc wiele
        public ICollection<BookCopy> BookCopies { get; set; }//lista kopii
        #endregion
    }
}