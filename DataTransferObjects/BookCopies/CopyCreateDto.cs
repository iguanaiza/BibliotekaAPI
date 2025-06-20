using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects.BookCopies
{
    public class CopyCreateDto
    {
        [Required(ErrorMessage = "Wprowadź sygnaturę egzemplarza (18 znaków).")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "Niepoprawna sygnatura: wpisz 18 znaków")]
        public string Signature { get; set; }

        [Required(ErrorMessage = "Wprowadź numer inwentarzowy (5 cyfr)")]
        [Range(10000, 99999, ErrorMessage = "Niepoprawny numer inwentarzowy: wpisz 5 cyfr")]
        public int InventoryNum { get; set; }

        [Required(ErrorMessage = "Wprowadź ID książki.")]
        public int BookId { get; set; }
    }
}
