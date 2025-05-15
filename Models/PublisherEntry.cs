using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class PublisherEntry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public List<BookEntry> Books { get; set; } = new();
    }
}
