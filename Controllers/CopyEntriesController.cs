using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public CopyEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyEntry>>> GetCopyEntries()
        {
            return await _context.CopyEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CopyEntry>> GetCopyEntry(int id)
        {
            //Database
            var CopyEntry = await _context.CopyEntries.FindAsync(id);

            if (CopyEntry == null)
            {
                return NotFound();
            }
            return CopyEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<CopyEntry>> PostCopyEntry(CopyEntry CopyEntry)
        {
            CopyEntry.Id = 0;

            _context.CopyEntries.Add(CopyEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetCopyEntry), new { id = CopyEntry.Id });

            return Created(resourceUrl, CopyEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCopyEntry(int id, [FromBody] CopyEntry CopyEntry)
        {
            if (id != CopyEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(CopyEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CopyEntryExists(id))
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

        private bool CopyEntryExists(int id)
        {
            return _context.CopyEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopyEntry(int id)
        {
            var CopyEntry = await _context.CopyEntries.FindAsync(id);

            if (CopyEntry == null)
            {
                return NotFound();
            }

            _context.CopyEntries.Remove(CopyEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
