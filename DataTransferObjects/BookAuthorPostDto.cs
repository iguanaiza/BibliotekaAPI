using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.DataTransferObjects
{
    public class BookAuthorPostDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
        public List<int> BooksIds { get; set; }
    }
}
