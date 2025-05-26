using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;
using BibliotekaAPI.DataTransferObjects;

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
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookPostDto BookPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = BookPostDto.Title,
                Year = BookPostDto.Year,
                Description = BookPostDto.Description,
                Isbn = BookPostDto.Isbn,
                PageCount = BookPostDto.PageCount,
                IsVisible = BookPostDto.IsVisible,
                BookAuthorId = BookPostDto.BookAuthorId,
                BookPublisherId = BookPostDto.BookPublisherId,
                BookSeriesId = BookPostDto.BookSeriesId,
                BookTypeId = BookPostDto.BookTypeId,
                BookCategoryId = BookPostDto.BookCategoryId,
                BookBookGenres = BookPostDto.BookGenreIds
                    .Select(id => new BookBookGenre
                    {
                        BookGenreId = id
                    })
                    .ToList()
                };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookPutDto BookPutDto)
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

            //nadpisanie tylko wybranego pola (można edytować np. sam tytuł)
            foreach (var prop in typeof(BookPutDto).GetProperties())
            {
                var value = prop.GetValue(BookPutDto);
                if (value != null)
                {
                    var bookProp = typeof(Book).GetProperty(prop.Name);
                    if (bookProp != null && bookProp.CanWrite)
                    {
                        bookProp.SetValue(book, value);
                    }
                }
            }

            /*book.Title = BookPutDto.Title;
            book.Year = BookPutDto.Year;
            book.Description = BookPutDto.Description;
            book.Isbn = BookPutDto.Isbn;
            book.PageCount = BookPutDto.PageCount;
            book.IsVisible = BookPutDto.IsVisible;
            book.BookAuthorId = BookPutDto.BookAuthorId;
            book.BookPublisherId = BookPutDto.BookPublisherId;
            book.BookSeriesId = BookPutDto.BookSeriesId;
            book.BookTypeId = BookPutDto.BookTypeId;
            book.BookCategoryId = BookPutDto.BookCategoryId;
            book.BookBookGenres = BookPutDto.BookGenreIds
                .Select(id => new BookBookGenre
                {
                    BookGenreId = id
                })
                .ToList();*/

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