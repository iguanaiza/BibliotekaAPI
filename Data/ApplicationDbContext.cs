using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<AuthorEntry> AuthorEntries { get; set; } //ustawienie jako zestawu danych
        public DbSet<BookEntry> BookEntries { get; set; } 
        public DbSet<BookTypeEntry> BookTypeEntries { get; set; }
        public DbSet<CategoryEntry> CategoryEntries { get; set; }
        public DbSet<CopyEntry> CopyEntries { get; set; }
        public DbSet<GenreEntry> GenreEntries { get; set; }
        public DbSet<PublisherEntry> PublisherEntries { get; set; }
        public DbSet<SeriesEntry> SeriesEntries { get; set; }
    }
}
