using System.ComponentModel.DataAnnotations;

namespace BibliotekaAPI.Models
{
    //genre czyli gatunek np. fantastyka - to nie kategoria ksiazki ani nie typ!!!
    public class BookGenre
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<BookBookGenre> BookBookGenres { get; set; }
    }
}