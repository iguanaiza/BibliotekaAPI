using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class BookAuthor
    {
        public int Id { get; set; } //ponoć nie trzeba zmienić, ale można rozważyć AuthorId i podobnie dla pozostałych

        public string Name { get; set; }
        
        public string Surname { get; set; }

        //kolekcja książek tego autora - relacja jeden do wielu, zalecany jednak ICollection dla EF
        //public List<Book> Books { get; set; } = [];
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
