using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //kategoria czyli lektura, podrecznik, inne - to nie gatunek ksiazki ani nie typ!!!
    public class CategoryEntry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public List<BookEntry> Books { get; set; } = new();
    }
}
