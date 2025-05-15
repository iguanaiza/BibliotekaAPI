using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")] //służy do definiowania adresu URL, pod którym będzie dostępny kontroler
    [ApiController]
    public class BookEntriesController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context; //odczyt kontekstu ??
        public BookEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet] //pobiera wszystko z bazy, dotyczacego klasy BookEntry wiec i podklas
        public async Task<ActionResult<IEnumerable<BookEntry>>> GetBookEntries()
        {
            return await _context.BookEntries
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Series)
                .Include(b => b.Type)
                .Include(b => b.Genre)
                .Include(b => b.Copy)
                .ToListAsync();
        }
        /*IEnumerable nie może być async, musi być Taskiem żeby być async.
        IEnumerable nie może jednak być Taskiem, musi być ActionResultem.
        Dlatego definiujemy IEnumerable jako ActionResult, a ten ActionResult jako Task*/

        [HttpGet("{id}")] //Odczytuje dany BookEntry rekord, musimy zdefiniować że chcemy konkretne ID pozyskać
        public async Task<ActionResult<BookEntry>> GetBookEntry(int id) //Musi być Task przy async
        {
            //Database
            var bookEntry = await _context.BookEntries.FindAsync(id);

            if (bookEntry == null)
            {
                return NotFound(); //Metoda z ActionResult
            }
            return bookEntry;
        }
        #endregion

        #region POST
        [HttpPost] //Tworzy nowy obiekt w bazie
        public async Task<ActionResult<BookEntry>> PostBookEntry(BookEntry BookEntry)
        {
            BookEntry.Id = 0; //Dzięki temu dajemy znać, że to NOWY wpis, a ID zostanie automatycznie nadane kolejno

            _context.BookEntries.Add(BookEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetBookEntry), new { id = BookEntry.Id }); //Tworzy URL dla nowego obiektu, jest to ważne

            return Created(resourceUrl, BookEntry); //zwraca nasz URL oraz obiekt
        }
        #endregion

        #region PUT
        //Update dla rekordu o zadanym id, URL pattern: api/BookEntries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookEntry(int id, [FromBody] BookEntry BookEntry) //binding parametru z body do BookEntry format
        {
            if (id != BookEntry.Id)
            {
                return BadRequest(); //Metoda z ActionResult
            }

            _context.Entry(BookEntry).State = EntityState.Modified; //oznacza nasz entry jako updated

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //ok, ale nie zwraca niczego
        }

        private bool BookEntryExists(int id) //funkcja pomocnicza do weryfikacji czy wpis z zadanym ID istnieje
        {
            return _context.BookEntries.Any(d => d.Id == id);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")] //Usuwa dany wpis, musimy zdefiniować id, URL pattern: api/BookEntries/{id}
        public async Task<IActionResult> DeleteBookEntry(int id)
        {
            var BookEntry = await _context.BookEntries.FindAsync(id);

            if (BookEntry == null)
            {
                return NotFound();
            }

            _context.BookEntries.Remove(BookEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
