using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;
using BibliotekaAPI.DataTransferObjects;
using BibliotekaAPI.DataTransferObjects.Books;
using BibliotekaAPI.Models.Params;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BibliotekaAPI.DataTransferObjects.BookCopies;


namespace BibliotekaAPI.Controllers
{
    [Route("api/catalog/[controller]")]
    [ApiController]
    public class CatalogController(ApplicationDbContext _context) : ControllerBase
    {
        #region BOOKS
        [HttpGet("books")]
        public async Task<ActionResult<BookGetDto>> GetBooks([FromQuery] BookQueryParams query)
        {
            var booksQuery = _context.Books
                .Where(b => !b.IsDeleted)
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres).ThenInclude(bb => bb.BookGenre)
                .Include(b => b.BookBookSpecialTags).ThenInclude(bb => bb.BookSpecialTag)
                .Include(b => b.BookCopies)
                .AsQueryable();

            // Filtrowanie
            if (!string.IsNullOrWhiteSpace(query.Title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(query.Title));

            if (query.Year.HasValue)
                booksQuery = booksQuery.Where(b => b.Year == query.Year);

            if (!string.IsNullOrWhiteSpace(query.Isbn))
                booksQuery = booksQuery.Where(b => b.Isbn.Contains(query.Isbn));

            if (!string.IsNullOrWhiteSpace(query.BookAuthor))
                booksQuery = booksQuery.Where(b => (b.BookAuthor != null) && (b.BookAuthor.Name.Contains(query.BookAuthor) || b.BookAuthor.Surname.Contains(query.BookAuthor)));

            if (!string.IsNullOrWhiteSpace(query.BookPublisher))
                booksQuery = booksQuery.Where(b => b.BookPublisher != null && b.BookPublisher.Name.Contains(query.BookPublisher));

            if (!string.IsNullOrWhiteSpace(query.BookSeries))
                booksQuery = booksQuery.Where(b => b.BookSeries != null && b.BookSeries.Title.Contains(query.BookSeries));

            if (!string.IsNullOrWhiteSpace(query.BookCategory))
                booksQuery = booksQuery.Where(b => b.BookCategory != null && b.BookCategory.Name.Contains(query.BookCategory));

            if (!string.IsNullOrWhiteSpace(query.BookType))
                booksQuery = booksQuery.Where(b => b.BookType != null && b.BookType.Title.Contains(query.BookType));

            if (query.BookGenres != null && query.BookGenres.Any())
                booksQuery = booksQuery.Where(b => b.BookBookGenres.Any(bg => query.BookGenres.Contains(bg.BookGenre.Title)));

            if (query.BookSpecialTags != null && query.BookSpecialTags.Any())
                booksQuery = booksQuery.Where(b => b.BookBookSpecialTags.Any(bg => query.BookSpecialTags.Contains(bg.BookSpecialTag.Title)));

            // Sortowanie
            bool descending = string.Equals(query.SortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            booksQuery = (query.SortBy?.ToLower()) switch
            {
                "title" => descending ? booksQuery.OrderByDescending(b => b.Title) : booksQuery.OrderBy(b => b.Title),
                "year" => descending ? booksQuery.OrderByDescending(b => b.Year) : booksQuery.OrderBy(b => b.Year),
                "isbn" => descending ? booksQuery.OrderByDescending(b => b.Isbn) : booksQuery.OrderBy(b => b.Isbn),
                "author" => descending ? booksQuery.OrderByDescending(b => b.BookAuthor!.Surname) : booksQuery.OrderBy(b => b.BookAuthor!.Surname),
                "publisher" => descending ? booksQuery.OrderByDescending(b => b.BookPublisher!.Name) : booksQuery.OrderBy(b => b.BookPublisher!.Name),
                "series" => descending ? booksQuery.OrderByDescending(b => b.BookSeries!.Title) : booksQuery.OrderBy(b => b.BookSeries!.Title),
                "category" => descending ? booksQuery.OrderByDescending(b => b.BookCategory!.Name) : booksQuery.OrderBy(b => b.BookCategory!.Name),
                "type" => descending ? booksQuery.OrderByDescending(b => b.BookType!.Title) : booksQuery.OrderBy(b => b.BookType!.Title),
                _ => booksQuery.OrderBy(b => b.Title)
            };

            // Paginacja
            var totalCount = await booksQuery.CountAsync();
            var items = await booksQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(b => new BookGetDto
                {
                    Title = b.Title,
                    Year = b.Year,
                    Description = b.Description,
                    Isbn = b.Isbn,
                    PageCount = b.PageCount,
                    ImageUrl = b.ImageUrl,
                    Subject = b.Subject,
                    Class = b.Class,
                    BookAuthor = b.BookAuthor.Surname + ", " + b.BookAuthor.Name,
                    BookPublisher = b.BookPublisher.Name,
                    BookSeries = b.BookSeries != null ? b.BookSeries.Title : null,
                    BookCategory = b.BookCategory.Name,
                    BookType = b.BookType.Title,
                    BookGenres = b.BookBookGenres.Select(bg => bg.BookGenre.Title).ToList(),
                    BookSpecialTags = b.BookBookSpecialTags.Select(bb => bb.BookSpecialTag.Title).ToList(),
                    CopyCount = b.BookCopies.Count(),
                    AvailableCopyCount = b.BookCopies.Count(c => c.Available)
                })
                .ToListAsync();

            return Ok(new
            {
                query.PageNumber,
                query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                Items = items
            });
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<BookGetDto>> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres)
                   .ThenInclude(bb => bb.BookGenre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var bookDto = new BookGetDto
            {
                Title = book.Title,
                Year = book.Year,
                Description = book.Description,
                Isbn = book.Isbn,
                PageCount = book.PageCount,
                ImageUrl = book.ImageUrl,
                Subject = book.Subject,
                Class = book.Class,
                BookAuthor = book.BookAuthor.Surname + ", " + book.BookAuthor.Name,
                BookPublisher = book.BookPublisher.Name,
                BookSeries = book.BookSeries.Title,
                BookType = book.BookType.Title,
                BookCategory = book.BookCategory.Name,
                BookGenres = book.BookBookGenres.Select(g => g.BookGenre.Title).ToList(),
                BookSpecialTags = book.BookBookSpecialTags.Select(bb => bb.BookSpecialTag.Title).ToList(),
                BookCopies = book.BookCopies?
                            .Select(c => new CopyGetDetailedDto
                            {
                                Id = c.Id,
                                Signature = c.Signature,
                                Available = c.Available,
                                InventoryNum = c.InventoryNum
                            }).ToList(),
                CopyCount = book.BookCopies != null ? book.BookCopies.Count : 0,
                AvailableCopyCount = book.BookCopies.Count(c => c.Available)
            };

            return Ok(bookDto);
        }

        #endregion

        #region COPIES
        [HttpGet("details/{bookId}/copies")]
        public async Task<ActionResult<CopyGetDto>> GetCopiesForBook(int bookId)
        {
            var copies = await _context.BookCopies
                .Where(c => c.BookId == bookId)
                .Include(c => c.Book).ThenInclude(b => b.BookAuthor)
                .Select(c => new CopyGetDto
                {
                    Signature = c.Signature,
                    Available = c.Available,
                    BookTitle = c.Book.Title,
                    BookImageUrl = c.Book.ImageUrl, 
                    AuthorName = c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name
                })
                .ToListAsync();

            if (copies == null || !copies.Any())
                return NotFound();

            return Ok(copies);
        }

        [HttpGet("details/{bookId}/copies/{copyId}")]
        public async Task<ActionResult<CopyGetDto>> GetCopyDetails(int bookId, int copyId)
        {
            var copy = await _context.BookCopies
                .Where(c => c.BookId == bookId && c.Id == copyId)
                .Include(c => c.Book).ThenInclude(b => b.BookAuthor)
                .Select(c => new CopyGetDto
                {
                    Signature = c.Signature,
                    Available = c.Available,
                    BookTitle = c.Book.Title,
                    BookImageUrl = c.Book.ImageUrl,
                    AuthorName = c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name
                })
                .FirstOrDefaultAsync();

            if (copy == null) return NotFound();

            return Ok(copy);
        }
        #endregion
    }
}
