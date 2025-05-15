using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //typ czyli powieść, nowela itp. - to nie gatunek ksiazki ani nie kategoria!!!
    public class BookTypeEntry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public List<BookEntry> Books { get; set; } = new();
    }
}
