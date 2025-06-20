using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Models.Singles;
using BibliotekaAPI.Models.Relations;

namespace BibliotekaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BookSpecialTag> BookSpecialTags { get; set; }
        public DbSet<BookPublisher> BookPublishers { get; set; }
        public DbSet<BookSeries> BookSeries { get; set; }
        public DbSet<BookType> BookTypes { get; set; }
        public DbSet<BookBookGenre> BooksBookGenres { get; set; }
        public DbSet<BookBookSpecialTag> BooksBookSpecialTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //filtr - pokazuje tylko ksiazki nieusuniete (tzn nieoznaczone jako usuniete)
            //modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);

            //polaczenie ksiazka-gatunek (wiele-do-wielu)
            modelBuilder.Entity<BookBookGenre>()
                .HasKey(bb => new { bb.BookId, bb.BookGenreId });

            modelBuilder.Entity<BookBookGenre>()
                .HasOne(bb => bb.Book)
                .WithMany(b => b.BookBookGenres)
                .HasForeignKey(bb => bb.BookId);

            modelBuilder.Entity<BookBookGenre>()
                .HasOne(bb => bb.BookGenre)
                .WithMany(bg => bg.BookBookGenres)
                .HasForeignKey(bb => bb.BookGenreId);

            //polaczenie ksiazka-tag (wiele-do-wielu)
            modelBuilder.Entity<BookBookSpecialTag>()
                .HasKey(bb => new { bb.BookId, bb.BookSpecialTagId });

            modelBuilder.Entity<BookBookSpecialTag>()
                .HasOne(bb => bb.Book)
                .WithMany(b => b.BookBookSpecialTags)
                .HasForeignKey(bb => bb.BookId);

            modelBuilder.Entity<BookBookSpecialTag>()
                .HasOne(bb => bb.BookSpecialTag)
                .WithMany(bg => bg.BookBookSpecialTags)
                .HasForeignKey(bb => bb.BookSpecialTagId);
        }
    }
}
