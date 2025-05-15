using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public CategoryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryEntry>>> GetCategoryEntries()
        {
            return await _context.CategoryEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryEntry>> GetCategoryEntry(int id)
        {
            //Database
            var CategoryEntry = await _context.CategoryEntries.FindAsync(id);

            if (CategoryEntry == null)
            {
                return NotFound();
            }
            return CategoryEntry;
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<CategoryEntry>> PostCategoryEntry(CategoryEntry CategoryEntry)
        {
            CategoryEntry.Id = 0;

            _context.CategoryEntries.Add(CategoryEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetCategoryEntry), new { id = CategoryEntry.Id });

            return Created(resourceUrl, CategoryEntry);
        }

        [HttpPost("batch")] //do przyjecia wielu naraz
        public async Task<IActionResult> AddMultipleCategories([FromBody] List<CategoryEntry> CategoryEntry)
        {
            _context.CategoryEntries.AddRange(CategoryEntry);
            await _context.SaveChangesAsync();
            return Ok(CategoryEntry);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryEntry(int id, [FromBody] CategoryEntry CategoryEntry)
        {
            if (id != CategoryEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(CategoryEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryEntryExists(id))
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

        private bool CategoryEntryExists(int id)
        {
            return _context.CategoryEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryEntry(int id)
        {
            var CategoryEntry = await _context.CategoryEntries.FindAsync(id);

            if (CategoryEntry == null)
            {
                return NotFound();
            }

            _context.CategoryEntries.Remove(CategoryEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
