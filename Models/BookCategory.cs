using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //kategoria czyli lektura, podrecznik, pozostałe - to nie gatunek ksiazki ani nie typ!!!
    public class BookCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
