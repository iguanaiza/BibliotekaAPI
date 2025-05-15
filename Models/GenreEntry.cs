using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //genre czyli gatunek np. fantastyka - to nie kategoria ksiazki ani nie typ!!!
    public class GenreEntry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public List<BookEntry> Books { get; set; } = new();
    }
}
