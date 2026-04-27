using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Repositories;

namespace LibraryBorrowingSystem.Services;

// TODO (Issue 2): Implement full business logic and caching
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            ISBN = b.ISBN,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies
        });
    }

    public async Task<BookResponseDto?> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null) return null;
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }

    public Task<BookResponseDto> CreateAsync(CreateBookDto dto) => throw new NotImplementedException("Issue 2");
    public Task<BookResponseDto?> UpdateAsync(int id, UpdateBookDto dto) => throw new NotImplementedException("Issue 2");
    public Task<bool> DeleteAsync(int id) => throw new NotImplementedException("Issue 2");
}
