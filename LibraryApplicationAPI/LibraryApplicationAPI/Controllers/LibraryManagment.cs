using LibraryApplicationAPI.Interfaces;
using LibraryApplicationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryManagment : ControllerBase
    {

        private readonly IBookRepository _repository;

        public LibraryManagment(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok(await _repository.GetAllBooksAsync());
        }

        [HttpGet]
        [Route("GetBook/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<ActionResult> AddBook(Book book)
        {
            await _repository.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpDelete]
        [Route("DeleteBook/{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                await _repository.DeleteBookAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
            
            return Ok("Book Deleted");
        }

        [HttpPost]
        [Route("LendBook/{id}/lend")]
        public async Task<ActionResult> LendBook(int id)
        {
            var success = await _repository.LendBookAsync(id);
            if (!success)
            {
                return BadRequest("Book not available.");
            }
            return Ok("Book lended");
        }

        [HttpPost]
        [Route("ReturnBook/{id}/return")]
        public async Task<ActionResult> ReturnBook(int id)
        {
            var success = await _repository.ReturnBookAsync(id);
            if (!success)
            {
                return BadRequest("Book not found.");
            }
            return Ok("Book Returned");
        }
    }
}
