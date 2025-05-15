using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public PublisherEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherEntry>>> GetPublisherEntries()
        {
            return await _context.PublisherEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherEntry>> GetPublisherEntry(int id)
        {
            //Database
            var PublisherEntry = await _context.PublisherEntries.FindAsync(id);

            if (PublisherEntry == null)
            {
                return NotFound();
            }
            return PublisherEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<PublisherEntry>> PostPublisherEntry(PublisherEntry PublisherEntry)
        {
            PublisherEntry.Id = 0;

            _context.PublisherEntries.Add(PublisherEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetPublisherEntry), new { id = PublisherEntry.Id });

            return Created(resourceUrl, PublisherEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisherEntry(int id, [FromBody] PublisherEntry PublisherEntry)
        {
            if (id != PublisherEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(PublisherEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherEntryExists(id))
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

        private bool PublisherEntryExists(int id)
        {
            return _context.PublisherEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisherEntry(int id)
        {
            var PublisherEntry = await _context.PublisherEntries.FindAsync(id);

            if (PublisherEntry == null)
            {
                return NotFound();
            }

            _context.PublisherEntries.Remove(PublisherEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
