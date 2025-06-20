using BibliotekaAPI.Models.Relations;

namespace BibliotekaAPI.Models.Singles
{
    public class BookSpecialTag
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public string Title { get; set; }
        public ICollection<BookBookSpecialTag>? BookBookSpecialTags { get; set; }
    }
}
