using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public GenreEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreEntry>>> GetGenreEntries()
        {
            return await _context.GenreEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreEntry>> GetGenreEntry(int id)
        {
            //Database
            var GenreEntry = await _context.GenreEntries.FindAsync(id);

            if (GenreEntry == null)
            {
                return NotFound();
            }
            return GenreEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<GenreEntry>> PostGenreEntry(GenreEntry GenreEntry)
        {
            GenreEntry.Id = 0;

            _context.GenreEntries.Add(GenreEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetGenreEntry), new { id = GenreEntry.Id });

            return Created(resourceUrl, GenreEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenreEntry(int id, [FromBody] GenreEntry GenreEntry)
        {
            if (id != GenreEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(GenreEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreEntryExists(id))
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

        private bool GenreEntryExists(int id)
        {
            return _context.GenreEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreEntry(int id)
        {
            var GenreEntry = await _context.GenreEntries.FindAsync(id);

            if (GenreEntry == null)
            {
                return NotFound();
            }

            _context.GenreEntries.Remove(GenreEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
