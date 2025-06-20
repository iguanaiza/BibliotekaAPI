using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models.Singles;
using BibliotekaAPI.Models.Params;
using BibliotekaAPI.DataTransferObjects.Books;
using BibliotekaAPI.DataTransferObjects.BookCopies;

namespace BibliotekaAPI.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class CopyController(ApplicationDbContext _context) : ControllerBase
    {
        #region GET
        [HttpGet("copies")]
        public async Task<ActionResult<IEnumerable<CopyGetDetailedDto>>> GetCopiesDetailed([FromQuery] CopyQueryParams query)
        {
            var copyQuery = _context.BookCopies
                .Where(c => !c.IsDeleted)
                .Include(c => c.Book).ThenInclude(b => b.BookAuthor)
                .AsQueryable();

            // Filtrowanie
            if (!string.IsNullOrWhiteSpace(query.Signature))
                copyQuery = copyQuery.Where(c => c.Signature.Contains(query.Signature));

            if (query.InventoryNum.HasValue)
                copyQuery = copyQuery.Where(c => c.InventoryNum == query.InventoryNum.Value);

            if (query.Available.HasValue)
                copyQuery = copyQuery.Where(c => c.Available == query.Available.Value);

            if (!string.IsNullOrWhiteSpace(query.BookTitle))
                copyQuery = copyQuery.Where(c => c.Book.Title.Contains(query.BookTitle));

            if (!string.IsNullOrWhiteSpace(query.AuthorName))
                copyQuery = copyQuery.Where(c => (c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name).Contains(query.AuthorName));

            // Sortowanie
            bool descending = string.Equals(query.SortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            copyQuery = (query.SortBy?.ToLower()) switch
            {
                "signature" => descending ? copyQuery.OrderByDescending(c => c.Signature) : copyQuery.OrderBy(c => c.Signature),
                "inventorynum" => descending ? copyQuery.OrderByDescending(c => c.InventoryNum) : copyQuery.OrderBy(c => c.InventoryNum),
                "available" => descending ? copyQuery.OrderByDescending(c => c.Available) : copyQuery.OrderBy(c => c.Available),
                "booktitle" => descending ? copyQuery.OrderByDescending(c => c.Book.Title) : copyQuery.OrderBy(c => c.Book.Title),
                "author" => descending ? copyQuery.OrderByDescending(c => c.Book.BookAuthor.Surname) : copyQuery.OrderBy(c => c.Book.BookAuthor.Surname),
                _ => copyQuery.OrderBy(c => c.Signature)
            };

            // Paginacja
            var totalCount = await copyQuery.CountAsync();
            var items = await copyQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new CopyGetDetailedDto
                {
                    Id = c.Id,
                    Signature = c.Signature,
                    InventoryNum = c.InventoryNum,
                    Available = c.Available,
                    BookTitle = c.Book.Title,
                    BookImageUrl = c.Book.ImageUrl,
                    AuthorName = c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name
                })
                .ToListAsync();

            return Ok(new
            {
                query.PageNumber,
                query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                Items = items
            });
        }

        [HttpGet("bin")]
        public async Task<ActionResult<IEnumerable<CopyGetDetailedDto>>> GetDeletedCopies()
        {
            var deletedCopies = await _context.BookCopies
                .Where(c => c.IsDeleted)
                .Include(c => c.Book).ThenInclude(b => b.BookAuthor)
                .Select(c => new CopyGetDetailedDto
                {
                    Id = c.Id,
                    Signature = c.Signature,
                    InventoryNum = c.InventoryNum,
                    Available = c.Available,
                    BookTitle = c.Book.Title,
                    BookImageUrl = c.Book.ImageUrl,
                    AuthorName = c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name
                })
                .ToListAsync();

            return Ok(deletedCopies);
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<CopyGetDetailedDto>> GetCopyByIdDetailed(int id)
        {
            var copy = await _context.BookCopies
                .Include(c => c.Book)
                    .ThenInclude(b => b.BookAuthor)
                .Where(c => c.Id == id)
                .Select(c => new CopyGetDetailedDto
                {
                    Id = c.Id,
                    Signature = c.Signature,
                    InventoryNum = c.InventoryNum,
                    Available = c.Available,
                    BookTitle = c.Book.Title,
                    BookImageUrl = c.Book.ImageUrl,
                    AuthorName = c.Book.BookAuthor.Surname + ", " + c.Book.BookAuthor.Name
                })
                .FirstOrDefaultAsync();

            if (copy == null)
                return NotFound();

            return Ok(copy);
        }
        #endregion

        #region POST
        [HttpPost("create")]
        public async Task<ActionResult<CopyGetDetailedDto>> CreateCopy([FromBody] CopyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var copy = new BookCopy
            {
                Signature = dto.Signature,
                InventoryNum = dto.InventoryNum,
                BookId = dto.BookId,
                Available = true,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsDeleted = false
            };

            _context.BookCopies.Add(copy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCopyByIdDetailed), new { id = copy.Id }, copy);
        }
        #endregion

        #region PUT
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<CopyGetDetailedDto>> EditCopy(int id, [FromBody] CopyEditDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var copy = await _context.BookCopies.FindAsync(id);
            if (copy == null || copy.IsDeleted) return NotFound();

            copy.Signature = dto.Signature;
            copy.InventoryNum = dto.InventoryNum;
            copy.BookId = dto.BookId;
            copy.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Soft delete - przeniesienie egzemplarza do "kosza"
        [HttpPut("bin/{id}")]
        public async Task<ActionResult<CopyGetDetailedDto>> SoftDeleteCopy(int id)
        {
            var copy = await _context.BookCopies.FindAsync(id);
            if (copy == null || copy.IsDeleted) return NotFound();

            copy.IsDeleted = true;
            copy.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region DELETE
        //hard delete - usuwa z bazy, ale tylko jeśli jest w koszu
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var copy = await _context.BookCopies.FindAsync(id);

            if (copy == null)
                return NotFound();

            if (copy.IsDeleted == false)
            {
                return Conflict("Egzemplarz musi najpierw zostać przeniesiony do kosza (soft delete), zanim zostanie trwale usunięty.");
            }

            _context.BookCopies.Remove(copy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
