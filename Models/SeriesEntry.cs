using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class SeriesEntry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public List<BookEntry> Books { get; set; } = new();
    }
}
