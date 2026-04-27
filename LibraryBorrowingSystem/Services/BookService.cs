using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Models;
using LibraryBorrowingSystem.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryBorrowingSystem.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemoryCache _cache;

    public BookService(IBookRepository bookRepository, IMemoryCache cache)
    {
        _bookRepository = bookRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        if (_cache.TryGetValue(BookCacheKeys.AllBooks, out IEnumerable<BookResponseDto>? cached) && cached is not null)
            return cached;

        var books = await _bookRepository.GetAllAsync();
        var result = books.Select(MapToDto).ToList();
        _cache.Set(BookCacheKeys.AllBooks, result);
        return result;
    }

    public async Task<BookResponseDto?> GetByIdAsync(int id)
    {
        var cacheKey = BookCacheKeys.ById(id);
        if (_cache.TryGetValue(cacheKey, out BookResponseDto? cached))
            return cached;

        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null) return null;

        var result = MapToDto(book);
        _cache.Set(cacheKey, result);
        return result;
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookDto dto)
    {
        Validate(dto.Title, dto.Author, dto.ISBN, dto.TotalCopies, dto.AvailableCopies);

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.AvailableCopies
        };

        var created = await _bookRepository.AddAsync(book);
        _cache.Remove(BookCacheKeys.AllBooks);
        return MapToDto(created);
    }

    public async Task<BookResponseDto?> UpdateAsync(int id, UpdateBookDto dto)
    {
        Validate(dto.Title, dto.Author, dto.ISBN, dto.TotalCopies, dto.AvailableCopies);

        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null) return null;

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = dto.AvailableCopies;

        var updated = await _bookRepository.UpdateAsync(book);
        _cache.Remove(BookCacheKeys.AllBooks);
        _cache.Remove(BookCacheKeys.ById(id));
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null) return false;

        await _bookRepository.DeleteAsync(book);
        _cache.Remove(BookCacheKeys.AllBooks);
        _cache.Remove(BookCacheKeys.ById(id));
        return true;
    }

    private static void Validate(string title, string author, string isbn, int totalCopies, int availableCopies)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author is required.");
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN is required.");
        if (totalCopies <= 0)
            throw new ArgumentException("TotalCopies must be greater than 0.");
        if (availableCopies < 0 || availableCopies > totalCopies)
            throw new ArgumentException("AvailableCopies must be between 0 and TotalCopies.");
    }

    private static BookResponseDto MapToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        ISBN = book.ISBN,
        TotalCopies = book.TotalCopies,
        AvailableCopies = book.AvailableCopies
    };
}
