using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;
using BibliotekaAPI.DataTransferObjects;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var book = await _context.Books
                 .Include(b => b.BookAuthor)
                 .Include(b => b.BookPublisher)
                 .Include(b => b.BookSeries)
                 .Include(b => b.BookType)
                 .Include(b => b.BookCategory)
                 .Include(b => b.BookGenres)
                 .Select(b => new BookGetDto
                 {
                     Title = b.Title,
                     Year = b.Year,
                     Description = b.Description,
                     Isbn = b.Isbn,
                     PageCount = b.PageCount,
                     BookAuthor = b.BookAuthor.Surname + ", " + b.BookAuthor.Name,
                     BookPublisher = b.BookPublisher.Name,
                     BookSeries = b.BookSeries.Title,
                     BookType = b.BookType.Title,
                     BookCategory = b.BookCategory.Name,
                     BookGenres = b.BookGenres
                 })
                 .ToListAsync();

            return Ok(book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return Ok(book);
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookPostDto BookPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = BookPostDto.Title,
                Year = BookPostDto.Year,
                Description = BookPostDto.Description,
                Isbn = BookPostDto.Isbn,
                PageCount = BookPostDto.PageCount,
                BookAuthorId = BookPostDto.BookAuthorId,
                BookPublisherId = BookPostDto.BookPublisherId,
                BookSeriesId = BookPostDto.BookSeriesId,
                BookTypeId = BookPostDto.BookTypeId,
                BookCategoryId = BookPostDto.BookCategoryId,
                BookGenres = await _context.BookGenres
                    .Where(g => BookPostDto.BookGenreIds.Contains(g.Id))
                    .ToListAsync()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] Book Book) 
        {
            if (id != Book.Id)
            {
                return BadRequest(); 
            }

            _context.Entry(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var Book = await _context.Books.FindAsync(id);

            if (Book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(Book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    } 
}
