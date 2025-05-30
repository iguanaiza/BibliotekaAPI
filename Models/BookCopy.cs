using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaAPI.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public string Signature { get; set; } //syngatura
        public string InventoryNum { get; set; } //numer inwentarzowy
        public bool Available { get; set; } //czy aktualnie dostepny tak/nie
        public int BookId { get; set; } //klucz ksiazki (oryginalu)

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } //odwolanie, musi miec oryginal
    }
}
