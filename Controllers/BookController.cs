using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.DataTransferObjects;
using BibliotekaAPI.DataTransferObjects.Books;
using BibliotekaAPI.Models.Params;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BibliotekaAPI.Models.Relations;
using BibliotekaAPI.Models.Singles;
using BibliotekaAPI.DataTransferObjects.BookCopies;

namespace BibliotekaAPI.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class BookController(ApplicationDbContext _context) : ControllerBase
    {
        #region GET
        [HttpGet("books")]
        public async Task<ActionResult<BookGetDetailedDto>> GetBooksDetailed([FromQuery] BookQueryParams query)
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
                booksQuery = booksQuery.Where(b => b.Title != null && b.Title.Contains(query.Title));

            if (query.Year.HasValue)
                booksQuery = booksQuery.Where(b => b.Year == query.Year);

            if (!string.IsNullOrWhiteSpace(query.Isbn))
                booksQuery = booksQuery.Where(b => b.Isbn != null && b.Isbn.Contains(query.Isbn));

            if (!string.IsNullOrWhiteSpace(query.BookAuthor))
                booksQuery = booksQuery.Where(b =>
                    b.BookAuthor != null &&
                    (
                        (b.BookAuthor.Name != null && b.BookAuthor.Name.Contains(query.BookAuthor)) ||
                        (b.BookAuthor.Surname != null && b.BookAuthor.Surname.Contains(query.BookAuthor))
                    )
                );

            if (!string.IsNullOrWhiteSpace(query.BookPublisher))
                booksQuery = booksQuery.Where(b =>
                    b.BookPublisher != null &&
                    b.BookPublisher.Name != null &&
                    b.BookPublisher.Name.Contains(query.BookPublisher)
                );

            if (!string.IsNullOrWhiteSpace(query.BookSeries))
                booksQuery = booksQuery.Where(b =>
                    b.BookSeries != null &&
                    b.BookSeries.Title != null &&
                    b.BookSeries.Title.Contains(query.BookSeries)
                );

            if (!string.IsNullOrWhiteSpace(query.BookCategory))
                booksQuery = booksQuery.Where(b =>
                    b.BookCategory != null &&
                    b.BookCategory.Name != null &&
                    b.BookCategory.Name.Contains(query.BookCategory)
                );

            if (!string.IsNullOrWhiteSpace(query.BookType))
                booksQuery = booksQuery.Where(b =>
                    b.BookType != null &&
                    b.BookType.Title != null &&
                    b.BookType.Title.Contains(query.BookType)
                );

            if (query.BookGenres != null && query.BookGenres.Count > 0)
                booksQuery = booksQuery.Where(b =>
                    b.BookBookGenres != null &&
                    b.BookBookGenres.Count(bg =>
                        bg.BookGenre != null &&
                        bg.BookGenre.Title != null &&
                        query.BookGenres.Contains(bg.BookGenre.Title)) > 0
                );

            if (query.BookSpecialTags != null && query.BookSpecialTags.Count > 0)
            {
                booksQuery = booksQuery.Where(b =>
                    b.BookBookSpecialTags != null &&
                    b.BookBookSpecialTags.Count(bg =>
                        bg.BookSpecialTag != null &&
                        query.BookSpecialTags.Contains(bg.BookSpecialTag.Title)) > 0);
            }

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
                .Select(b => new BookGetDetailedDto
                 {
                     Id = b.Id,
                     Title = b.Title,
                     Year = b.Year,
                     Description = b.Description,
                     Isbn = b.Isbn,
                     PageCount = b.PageCount,
                     IsDeleted = b.IsDeleted,
                     IsVisible = b.IsVisible,
                     ImageUrl = b.ImageUrl,
                     Subject = b.Subject,
                     Class = b.Class,
                     BookAuthor = b.BookAuthor.Surname + ", " + b.BookAuthor.Name,
                     BookPublisher = b.BookPublisher.Name,
                     BookSeries = b.BookSeries != null ? b.BookSeries.Title : null,
                     BookCategory = b.BookCategory.Name,
                     BookType = b.BookType.Title,
                     BookGenres = b.BookBookGenres.Select(bg => bg.BookGenre.Title).ToList(),
                     BookSpecialTags = b.BookBookSpecialTags != null
                        ? b.BookBookSpecialTags
                            .Where(bb => bb.BookSpecialTag != null)
                            .Select(bb => bb.BookSpecialTag!.Title)
                            .ToList()
                        : new List<string>(),
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

        [HttpGet("bin")]
        public async Task<ActionResult<BookGetDetailedDto>> GetBooksDeleted()
        {
            var books = await _context.Books
                .Where(b => b.IsDeleted)
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres)
                    .ThenInclude(bb => bb.BookGenre)
                .Select(b => new BookGetDetailedDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Description = b.Description,
                    Isbn = b.Isbn,
                    PageCount = b.PageCount,
                    IsVisible = b.IsVisible,
                    BookAuthor = $"{b.BookAuthor.Surname}, {b.BookAuthor.Name}",
                    BookPublisher = b.BookPublisher.Name,
                    BookSeries = b.BookSeries != null ? b.BookSeries.Title : null,
                    BookType = b.BookType.Title,
                    BookCategory = b.BookCategory.Name,
                    BookGenres = b.BookBookGenres.Select(bb => bb.BookGenre.Title).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<BookGetDetailedDto>> GetBookByIdDetailed(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres).ThenInclude(bb => bb.BookGenre)
                .Include(b => b.BookBookSpecialTags).ThenInclude(bb => bb.BookSpecialTag)
                .Include(b => b.BookCopies)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var bookDto = new BookGetDetailedDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Description = book.Description,
                Isbn = book.Isbn,
                PageCount = book.PageCount,
                IsVisible = book.IsVisible,
                ImageUrl = book.ImageUrl,
                Subject = book.Subject,
                Class = book.Class,
                BookAuthor = book.BookAuthor.Surname + ", " + book.BookAuthor.Name,
                BookPublisher = book.BookPublisher.Name,
                BookSeries = book.BookSeries.Title,
                BookType = book.BookType.Title,
                BookCategory = book.BookCategory.Name,
                BookGenres = book.BookBookGenres.Select(g => g.BookGenre.Title).ToList(),
                BookSpecialTags = book.BookBookSpecialTags?
                    .Where(bb => bb.BookSpecialTag != null)
                    .Select(bb => bb.BookSpecialTag!.Title)
                    .ToList() ?? new List<string>(),
                BookCopies = book.BookCopies?
                            .Select(c => new CopyGetDetailedDto
                            {
                                Id = c.Id,
                                Signature = c.Signature,
                                Available = c.Available,
                                InventoryNum = c.InventoryNum
                            }).ToList(),
                CopyCount = book.BookCopies != null ? book.BookCopies.Count : 0,
                AvailableCopyCount = book.BookCopies?.Count(c => c.Available) ?? 0
            };

            return Ok(bookDto);
        }
        #endregion

        #region POST
        [HttpPost("create")]
        public async Task<ActionResult<BookGetDetailedDto>> CreateBook([FromBody] BookCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int defaultSeriesId = 3; // "brak"

            var book = new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                Description = dto.Description,
                Isbn = dto.Isbn,
                PageCount = dto.PageCount,
                BookAuthorId = dto.BookAuthorId,
                BookPublisherId = dto.BookPublisherId,
                BookSeriesId = dto.BookSeriesId ?? defaultSeriesId,
                BookTypeId = dto.BookTypeId,
                BookCategoryId = dto.BookCategoryId,
                IsVisible = true,
                IsDeleted = false,
                BookBookGenres = dto.BookGenreIds?
                    .Select(id => new BookBookGenre { BookGenreId = id })
                    .ToList() ?? new List<BookBookGenre>(),
                BookBookSpecialTags = dto.BookSpecialTagIds?
                    .Select(id => new BookBookSpecialTag { BookSpecialTagId = id })
                    .ToList() ?? new List<BookBookSpecialTag>()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookByIdDetailed), new { id = book.Id }, book);
        }
        #endregion

        #region PUT
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<BookGetDetailedDto>> EditBook(int id, [FromBody] BookEditDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books
                .Include(b => b.BookBookGenres)
                .Include(b => b.BookBookSpecialTags)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            book.Title = dto.Title;
            book.Year = dto.Year;
            book.Description = dto.Description;
            book.Isbn = dto.Isbn;
            book.PageCount = dto.PageCount;
            book.IsVisible = dto.IsVisible;
            book.BookAuthorId = dto.BookAuthorId;
            book.BookPublisherId = dto.BookPublisherId;
            book.BookSeriesId = dto.BookSeriesId;
            book.BookTypeId = dto.BookTypeId;
            book.BookCategoryId = dto.BookCategoryId;

            book.BookBookGenres.Clear();
            if (dto.BookGenreIds != null)
            {
                book.BookBookGenres = dto.BookGenreIds
                    .Select(id => new BookBookGenre { BookId = book.Id, BookGenreId = id })
                    .ToList();
            }

            book.BookBookSpecialTags.Clear();
            if (dto.BookSpecialTagIds != null)
            {
                book.BookBookSpecialTags = dto.BookSpecialTagIds
                    .Select(id => new BookBookSpecialTag { BookId = book.Id, BookSpecialTagId = id })
                    .ToList();
            }

            book.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Soft delete - przeniesienie książki do "kosza"
        [HttpPut("bin/{id}")]
        public async Task<ActionResult<BookGetDetailedDto>> BinBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.IsDeleted)
            {
                return Conflict("Książka już znajduje się w koszu.");
            }

            book.IsDeleted = true;
            book.IsVisible = false;
            book.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region DELETE
        //hard delete - usuwa z bazy, ale tylko jeśli jest w koszu
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookCopies)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.IsDeleted == false)
            {
                return Conflict("Książka musi najpierw zostać przeniesiona do kosza (soft delete), zanim zostanie trwale usunięta.");
            }

            if (book.BookCopies.Count > 0)
            {
                return Conflict("Nie można usunąć książki, ponieważ posiada przypisane egzemplarze.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    } 
}