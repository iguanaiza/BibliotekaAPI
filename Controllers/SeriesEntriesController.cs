using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public SeriesEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeriesEntry>>> GetSeriesEntries()
        {
            return await _context.SeriesEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeriesEntry>> GetSeriesEntry(int id)
        {
            //Database
            var SeriesEntry = await _context.SeriesEntries.FindAsync(id);

            if (SeriesEntry == null)
            {
                return NotFound();
            }
            return SeriesEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<SeriesEntry>> PostSeriesEntry(SeriesEntry SeriesEntry)
        {
            SeriesEntry.Id = 0;

            _context.SeriesEntries.Add(SeriesEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetSeriesEntry), new { id = SeriesEntry.Id });

            return Created(resourceUrl, SeriesEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeriesEntry(int id, [FromBody] SeriesEntry SeriesEntry)
        {
            if (id != SeriesEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(SeriesEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesEntryExists(id))
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

        private bool SeriesEntryExists(int id)
        {
            return _context.SeriesEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeriesEntry(int id)
        {
            var SeriesEntry = await _context.SeriesEntries.FindAsync(id);

            if (SeriesEntry == null)
            {
                return NotFound();
            }

            _context.SeriesEntries.Remove(SeriesEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
