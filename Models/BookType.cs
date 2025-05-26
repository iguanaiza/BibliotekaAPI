using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //typ czyli powieść, nowela itp. - to nie gatunek ksiazki ani nie kategoria!!!
    public class BookType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
