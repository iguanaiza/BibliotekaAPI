using BibliotekaAPI.Models.Relations;
using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models.Singles
{
    public class BookGenre
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public string Title { get; set; }
        public ICollection<BookBookGenre> BookBookGenres { get; set; }
    }
}