using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopyController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public BookCopyController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCopy>>> GetBookCopies()
        {
            return await _context.BookCopies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookCopy>> GetBookCopy(int id)
        {
            //Database
            var BookCopy = await _context.BookCopies.FindAsync(id);

            if (BookCopy == null)
            {
                return NotFound();
            }
            return BookCopy;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<BookCopy>> PostBookCopy(BookCopy BookCopy)
        {
            BookCopy.Id = 0;

            _context.BookCopies.Add(BookCopy);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetBookCopy), new { id = BookCopy.Id });

            return Created(resourceUrl, BookCopy);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookCopy(int id, [FromBody] BookCopy BookCopy)
        {
            if (id != BookCopy.Id)
            {
                return BadRequest();
            }

            _context.Entry(BookCopy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookCopyExists(id))
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

        private bool BookCopyExists(int id)
        {
            return _context.BookCopies.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookCopy(int id)
        {
            var BookCopy = await _context.BookCopies.FindAsync(id);

            if (BookCopy == null)
            {
                return NotFound();
            }

            _context.BookCopies.Remove(BookCopy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
