using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

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
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthors()
        {
            return await _context.BookAuthors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookAuthor>> GetBookAuthor(int id)
        {
            //Database
            var BookAuthor = await _context.BookAuthors.FindAsync(id);

            if (BookAuthor == null)
            {
                return NotFound();
            }
            return BookAuthor;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor BookAuthor)
        {
            BookAuthor.Id = 0;

            _context.BookAuthors.Add(BookAuthor);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetBookAuthor), new { id = BookAuthor.Id });

            return Created(resourceUrl, BookAuthor);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAuthor(int id, [FromBody] BookAuthor BookAuthor)
        {
            if (id != BookAuthor.Id)
            {
                return BadRequest();
            }

            _context.Entry(BookAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorExists(id))
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

        private bool BookAuthorExists(int id)
        {
            return _context.BookAuthors.Any(d => d.Id == id);
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
