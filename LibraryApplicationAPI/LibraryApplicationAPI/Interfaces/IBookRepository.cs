using LibraryApplicationAPI.Models;

namespace LibraryApplicationAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> LendBookAsync(int id);
        Task<bool> ReturnBookAsync(int id);
    }
}
