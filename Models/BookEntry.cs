using System.ComponentModel.DataAnnotations; //do używania [Required] czyli walidacji danych

namespace BibliotekaAPI.Models
{
    //klasa dot. ksiazki i jej danych
    public class BookEntry
    {
        public int Id { get; set; } //[Required] dla typów wartościowych (int, bool) nie ma sensu — one zawsze mają wartość domyślną (0, false).
        public DateTime Created { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Wprowadź tytuł.")] //dodać potem kolejne walidacje
        [MinLength(3, ErrorMessage = "Tytuł musi mieć co najmniej 3 znaki.")] //dodać potem kolejne walidacje
        public string Title { get; set; } = null!; //znak ! niby deklaruje, że nie jest null - do pozbycia się błędu CS8618

        public int Year { get; set; }

        public string Description { get; set; } = null!; //krótki opis ksiazki

        public int Isbn { get; set; } //numer ISBN

        public int PageCount { get; set; } //ilość stron

        #region Odwołania do innych
        public int AuthorId { get; set; }
        public AuthorEntry Author { get; set; } = null!; //autor

        public int PublisherId { get; set; }
        public PublisherEntry Publisher { get; set; } = null!; //wydawca

        public int SeriesId { get; set; }
        public SeriesEntry Series { get; set; } = null!; //seria np. Harry Potter

        public int BookTypeId { get; set; }
        public BookTypeEntry Type { get; set; } = null!; //typ ksiazki, rodzaj np. powieść

        public int CategoryId { get; set; }
        public CategoryEntry Category { get; set; } = null!; //jedno z trzech: lektura, podręcznik, pozostałe

        public List<GenreEntry> Genre { get; set; } = new(); //gatunek ksiazki np. fantasy - moze byc wiele
        public List<CopyEntry> Copy { get; set; } = new(); //kopie tej samej ksiazki np 5 sztuk
        #endregion
    }
}