using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects.Books
{
    public class BookEditDto
    {
        [Required(ErrorMessage = "Wprowadź tytuł książki (maksymalnie 100 znaków).")]
        [StringLength(100, ErrorMessage = "Niepoprawny tytuł: wpisz maksymalnie 100 znaków")]
        public required string Title { get; set; }


        [Required(ErrorMessage = "Wprowadź rok wydania książki (zakres 1000-2200.")]
        [Range(1000, 2200, ErrorMessage = "Niepoprawny rok: pisz cyfry z zakresu 1000-2200.")]
        public int Year { get; set; }


        [Required(ErrorMessage = "Wprowadź krótki opis książki (maksymalnie 255 znaków).")]
        [StringLength(255, ErrorMessage = "Niepoprawny opis: wpisz maksymalnie 255 znaków")]
        public required string Description { get; set; }


        [Required(ErrorMessage = "Wprowadź numer ISBN książki (13 cyfr w formacie XXX-XX-XXXX-XXX-X).")]
        [StringLength(17, ErrorMessage = "Niepoprawny ISBN: wprowadź 13 cyfr w formacie XXX-XX-XXXX-XXX-X.")]
        public required string Isbn { get; set; }

        public bool IsVisible { get; set; }

        [Required(ErrorMessage = "Wprowadź ilość stron w książce (zakres 1-1500).")]
        [Range(1, 1500, ErrorMessage = "Niepoprawna ilość stron: wpisz cyfry z zakresu 1-1500.")]
        public int PageCount { get; set; }

        public string? ImageUrl { get; set; }

        public string? Subject { get; set; }

        public string? Class { get; set; }

        [Required(ErrorMessage = "Wprowadź ID autora książki.")]
        public int BookAuthorId { get; set; }


        [Required(ErrorMessage = "Wprowadź ID wydawcy książki.")]
        public int BookPublisherId { get; set; }


        [Required(ErrorMessage = "Wprowadź ID serii, do której należy książka.")]
        public int BookSeriesId { get; set; }


        [Required(ErrorMessage = "Wprowadź ID rodzajów książki.")]
        public int BookTypeId { get; set; }


        [Required(ErrorMessage = "Wprowadź ID kategorii książki.")]
        public int BookCategoryId { get; set; }


        [Required(ErrorMessage = "Wprowadź ID gatunków książki.")]
        public required List<int> BookGenreIds { get; set; }

        public List<int>? BookSpecialTagIds { get; set; }
    }
}
