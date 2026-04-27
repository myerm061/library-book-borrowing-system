// IBookRepository.cs

using LibraryBorrowingSystem.Models;

namespace LibraryBorrowingSystem.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIdForUpdateAsync(int id);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<bool> ExistsAsync(int id);
}
