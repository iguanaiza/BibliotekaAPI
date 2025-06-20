namespace BibliotekaAPI.DataTransferObjects.BookCopies
{
    public class CopyGetDetailedDto
    {
        public int Id { get; set; }
        public string Signature { get; set; } = null!;
        public int InventoryNum { get; set; }
        public bool Available { get; set; }
        public string BookTitle { get; set; } = null!;
        public string? BookImageUrl { get; set; }
        public string? AuthorName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
