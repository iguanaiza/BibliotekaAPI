namespace BibliotekaAPI.DataTransferObjects.BookCopies
{
    public class CopyGetDto
    {
        public string Signature { get; set; } = null!;
        public bool Available { get; set; }
        public string BookTitle { get; set; } = null!;
        public string? BookImageUrl { get; set; }
        public string? AuthorName { get; set; }
    }
}
