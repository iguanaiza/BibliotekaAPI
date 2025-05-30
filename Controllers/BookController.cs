using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;
using BibliotekaAPI.DataTransferObjects;
using BibliotekaSzkolnaBlazor.DataTransferObjects;

namespace BibliotekaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region DbContext
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            /* Include zawiera powiazane encje np autor, wydawca itd dajac dostep do odwolan
             * select tworzy projekcje na moje Dto, czyli tylko to co chce zeby user widzial (enkapsulacja itd)
             * teoretycznie EntityFramewrok sam te relacje wlaczy jesli pomine .Select ale lepiej od razu to wpisac np usprawnia ladowanie danych
             */
            
            var book = await _context.Books
                 .Include(b => b.BookAuthor)
                 .Include(b => b.BookPublisher)
                 .Include(b => b.BookSeries)
                 .Include(b => b.BookType)
                 .Include(b => b.BookCategory)
                 .Include(b => b.BookBookGenres)
                    .ThenInclude(bb => bb.BookGenre)
                 .Select(b => new BookGetDto
                 {
                     Id = b.Id,
                     Title = b.Title,
                     Year = b.Year,
                     Description = b.Description,
                     Isbn = b.Isbn,
                     PageCount = b.PageCount,
                     IsVisible = b.IsVisible,
                     BookAuthor = b.BookAuthor.Surname + ", " + b.BookAuthor.Name,
                     BookPublisher = b.BookPublisher.Name,
                     BookSeries = b.BookSeries.Title,
                     BookType = b.BookType.Title,
                     BookCategory = b.BookCategory.Name,
                     BookGenres = b.BookBookGenres
                        .Select(bb => bb.BookGenre.Title)
                        .ToList()
                 })
                 .ToListAsync();

            return Ok(book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres)
                   .ThenInclude(bb => bb.BookGenre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            /* W tym przypadku tworzę moj obiekt Dto osobno, bo pojedynczy rekord (a w GET wszystkich to do jednej listy)
             */
            var bookDto = new BookGetDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Description = book.Description,
                Isbn = book.Isbn,
                PageCount = book.PageCount,
                IsVisible = book.IsVisible,
                BookAuthor = book.BookAuthor.Surname + ", " + book.BookAuthor.Name,
                BookPublisher = book.BookPublisher.Name,
                BookSeries = book.BookSeries.Title,
                BookType = book.BookType.Title,
                BookCategory = book.BookCategory.Name,
                BookGenres = book.BookBookGenres
                   .Select(b => b.BookGenre.Title)
                   .ToList()
            };

            return Ok(bookDto);
        }
        #endregion

        #region POST
        //public async Task<IActionResult> CreateBook(BookPostDto BookPostDto)
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                Description = dto.Description,
                Isbn = dto.Isbn,
                PageCount = dto.PageCount,
                IsVisible = dto.IsVisible,
                BookAuthorId = dto.BookAuthorId,
                BookPublisherId = dto.BookPublisherId,
                BookSeriesId = dto.BookSeriesId,
                BookTypeId = dto.BookTypeId,
                BookCategoryId = dto.BookCategoryId,
                BookBookGenres = dto.BookGenreIds.Select(id => new BookBookGenre { BookGenreId = id }).ToList()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        #endregion

        #region PUT
        // public async Task<IActionResult> UpdateBook(int id, [FromBody] BookPutDto BookPutDto)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .Include(b => b.BookSeries)
                .Include(b => b.BookType)
                .Include(b => b.BookCategory)
                .Include(b => b.BookBookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            book.Title = dto.Title;
            book.Year = dto.Year;
            book.Description = dto.Description;
            book.Isbn = dto.Isbn;
            book.PageCount = dto.PageCount;
            book.IsVisible = dto.IsVisible;
            book.BookAuthorId = dto.BookAuthorId;
            book.BookPublisherId = dto.BookPublisherId;
            book.BookSeriesId = dto.BookSeriesId;
            book.BookTypeId = dto.BookTypeId;
            book.BookCategoryId = dto.BookCategoryId;
            book.BookBookGenres = dto.BookGenreIds
                .Select(id => new BookBookGenre
                {
                    BookGenreId = id
                })
                .ToList();

            await _context.SaveChangesAsync();
            return Ok(book); //jeszcze mozna dac ew. return NoContent() - ale to nie zwraca tych zaktualizowanych danych
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            //book.IsDeleted = true; //soft delete - nie usuwa z bazy tylko wylacza
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    } 
}
/*
 *   "bookAuthorId": 1,
  "bookPublisherId": 1,
  "bookSeriesId": 1,
  "bookTypeId": 1,
  "bookCategoryId": 3,
  "bookGenreIds": [
    1, 2, 3
  ]

 "title": "Opowieści z Narnii: Srebrne krzesło",
  "year": 2008,
  "description": "W tomie czwartym zatytułowanym "Srebrne krzesło" Eustachy i Julia obdarzeni przez Aslana misją odnalezienia zaginionego królewicza Narnii, wędrując w towarzystwie dzielnego Błotosmętka przez budzące zdumienie i grozę krainy, odkrywają, że przeciwstawienie się złu daje zadziwiającą siłę do walki z nim.",
  "isbn": "978-83-7278-183-3",
  "pageCount": 234,
  "isVisible": true,
  "bookAuthorId": 2,
  "bookPublisherId": 1,
  "bookSeriesId": 2,
  "bookTypeId": 1,
  "bookCategoryId": 3,
  "bookGenreIds": [
    1, 2, 3
  ]
 */