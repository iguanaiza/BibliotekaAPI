using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookTypeEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public BookTypeEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookTypeEntry>>> GetBookTypeEntries()
        {
            return await _context.BookTypeEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookTypeEntry>> GetBookTypeEntry(int id)
        {
            //Database
            var BookTypeEntry = await _context.BookTypeEntries.FindAsync(id);

            if (BookTypeEntry == null)
            {
                return NotFound();
            }
            return BookTypeEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<BookTypeEntry>> PostBookTypeEntry(BookTypeEntry BookTypeEntry)
        {
            BookTypeEntry.Id = 0;

            _context.BookTypeEntries.Add(BookTypeEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetBookTypeEntry), new { id = BookTypeEntry.Id });

            return Created(resourceUrl, BookTypeEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookTypeEntry(int id, [FromBody] BookTypeEntry BookTypeEntry)
        {
            if (id != BookTypeEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(BookTypeEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookTypeEntryExists(id))
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

        private bool BookTypeEntryExists(int id)
        {
            return _context.BookTypeEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookTypeEntry(int id)
        {
            var BookTypeEntry = await _context.BookTypeEntries.FindAsync(id);

            if (BookTypeEntry == null)
            {
                return NotFound();
            }

            _context.BookTypeEntries.Remove(BookTypeEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
