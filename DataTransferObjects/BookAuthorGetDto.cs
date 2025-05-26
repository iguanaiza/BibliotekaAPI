namespace BibliotekaAPI.DataTransferObjects
{
    public class BookAuthorGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Books { get; set; }
    }
}
