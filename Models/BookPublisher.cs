using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    public class BookPublisher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
