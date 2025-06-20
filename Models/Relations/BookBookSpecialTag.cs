using BibliotekaAPI.Models.Singles;

namespace BibliotekaAPI.Models.Relations
{
    public class BookBookSpecialTag
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public int BookSpecialTagId { get; set; }
        public BookSpecialTag? BookSpecialTag { get; set; }
    }
}
