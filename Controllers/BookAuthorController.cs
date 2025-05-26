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
    public class BookAuthorController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public BookAuthorController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetBookAuthor()
        {
            var author = await _context.BookAuthors
                 .Include(a => a.Books)
                 .Select(a => new BookAuthorGetDto
                 {
                     Id = a.Id,
                     Name = a.Name,
                     Surname = a.Surname,
                     Books = a.Books
                        .Select(aa => aa.Title)
                        .ToList()
                 })
                 .ToListAsync();

            return Ok(author);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.BookAuthors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            var authorDto = new BookAuthorGetDto
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                Books = author.Books
                   .Select(a => a.Title)
                   .ToList()
            };

            return Ok(authorDto);
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(BookAuthorPostDto BookAuthorPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = new BookAuthor
            {
                Name = BookAuthorPostDto.Name,
                Surname = BookAuthorPostDto.Surname
            };

            _context.BookAuthors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] BookAuthorPutDto BookAuthorPutDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _context.BookAuthors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            foreach (var prop in typeof(BookAuthorPutDto).GetProperties())
            {
                var value = prop.GetValue(BookAuthorPutDto);
                if (value != null)
                {
                    var authorProp = typeof(Book).GetProperty(prop.Name);
                    if (authorProp != null && authorProp.CanWrite)
                    {
                        authorProp.SetValue(author, value);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(author);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAuthor(int id)
        {
            var BookAuthor = await _context.BookAuthors.FindAsync(id);

            if (BookAuthor == null)
            {
                return NotFound();
            }

            _context.BookAuthors.Remove(BookAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
