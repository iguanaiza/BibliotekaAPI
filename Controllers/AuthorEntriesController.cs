using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public AuthorEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorEntry>>> GetAuthorEntries()
        {
            return await _context.AuthorEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorEntry>> GetAuthorEntry(int id)
        {
            //Database
            var AuthorEntry = await _context.AuthorEntries.FindAsync(id);

            if (AuthorEntry == null)
            {
                return NotFound();
            }
            return AuthorEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<AuthorEntry>> PostAuthorEntry(AuthorEntry AuthorEntry)
        {
            AuthorEntry.Id = 0;

            _context.AuthorEntries.Add(AuthorEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetAuthorEntry), new { id = AuthorEntry.Id });

            return Created(resourceUrl, AuthorEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthorEntry(int id, [FromBody] AuthorEntry AuthorEntry)
        {
            if (id != AuthorEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(AuthorEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorEntryExists(id))
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

        private bool AuthorEntryExists(int id)
        {
            return _context.AuthorEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorEntry(int id)
        {
            var AuthorEntry = await _context.AuthorEntries.FindAsync(id);

            if (AuthorEntry == null)
            {
                return NotFound();
            }

            _context.AuthorEntries.Remove(AuthorEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
