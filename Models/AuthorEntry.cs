using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class AuthorEntry
    {
        public int Id { get; set; } //ponoć nie trzeba zmienić, ale można rozważyć AuthorId i podobnie dla pozostałych

        [Required]
        public string Name { get; set; } = null!;
        
        [Required]
        public string Surname { get; set; } = null!;

        //kolekcja książek tego autora - relacja jeden do wielu
        public List<BookEntry> Books { get; set; } = new();
    }
}
