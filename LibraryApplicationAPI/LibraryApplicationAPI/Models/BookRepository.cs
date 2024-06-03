using LibraryApplicationAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApplicationAPI.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> LendBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null && book.CopiesAvailable > 0)
            {
                book.CopiesAvailable--;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ReturnBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.CopiesAvailable++;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
