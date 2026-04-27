using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;

namespace LibraryBorrowingSystem.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto?> GetByIdAsync(int id);
    Task<BookResponseDto> CreateAsync(CreateBookDto dto);
    Task<BookResponseDto?> UpdateAsync(int id, UpdateBookDto dto);
    Task<bool> DeleteAsync(int id);
}
