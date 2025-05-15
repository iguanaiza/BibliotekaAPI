namespace BibliotekaAPI.Models
{
    public class CopyEntry
    {
        public int Id { get; set; }
        public bool Available { get; set; }

        public int BookEntryId { get; set; } //klucz ksiazki (oryginalu jakby)
        public BookEntry BookEntry { get; set; } = null!; //odwolanie
    }
}
