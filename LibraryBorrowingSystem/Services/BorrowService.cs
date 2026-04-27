// BorrowService.cs

using System.Collections.Concurrent;
using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Exceptions;
using LibraryBorrowingSystem.Models;
using LibraryBorrowingSystem.Repositories;
using Microsoft.Extensions.Caching.Memory;

// namespace(s)
namespace LibraryBorrowingSystem.Services;

public class BorrowService : IBorrowService
{
    private const string BorrowedStatus = "Borrowed";
    private const string ReturnedStatus = "Returned";
    private static readonly ConcurrentDictionary<int, SemaphoreSlim> BookLocks = new();
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemoryCache _cache;

    public BorrowService(
        IBorrowRepository borrowRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IMemoryCache cache)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync()
    {
        var records = await _borrowRepository.GetAllAsync();
        return records.Select(MapToDto);
    }

    public async Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId)
    {
        ValidateId(memberId, "MemberId must be greater than 0.");

        if (!await _memberRepository.ExistsAsync(memberId))
        {
            throw new NotFoundException("Member not found.");
        }

        var records = await _borrowRepository.GetByMemberIdAsync(memberId);
        return records.Select(MapToDto);
    }

    public async Task<BorrowRecordResponseDto> BorrowBookAsync(BorrowRequestDto dto)
    {
        ValidateBorrowRequest(dto.BookId, dto.MemberId);

        if (!await _memberRepository.ExistsAsync(dto.MemberId))
        {
            throw new NotFoundException("Member not found.");
        }

        var bookLock = BookLocks.GetOrAdd(dto.BookId, _ => new SemaphoreSlim(1, 1));
        await bookLock.WaitAsync();

        try
        {
            var book = await _bookRepository.GetByIdForUpdateAsync(dto.BookId);
            if (book is null)
            {
                throw new NotFoundException("Book not found.");
            }

            if (book.AvailableCopies <= 0)
            {
                throw new ConflictException("No available copies for this book.");
            }

            book.AvailableCopies--;

            var record = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.UtcNow,
                Status = BorrowedStatus
            };

            var createdRecord = await _borrowRepository.AddAsync(record);
            InvalidateBookCache(dto.BookId);
            return MapToDto(createdRecord);
        }
        finally
        {
            bookLock.Release();
        }
    }

    public async Task<BorrowRecordResponseDto> ReturnBookAsync(ReturnRequestDto dto)
    {
        ValidateBorrowRequest(dto.BookId, dto.MemberId);
        if (!await _memberRepository.ExistsAsync(dto.MemberId))
        {
            throw new NotFoundException("Member not found.");
        }
        var bookLock = BookLocks.GetOrAdd(dto.BookId, _ => new SemaphoreSlim(1, 1));
        await bookLock.WaitAsync();

        try
        {
            var book = await _bookRepository.GetByIdForUpdateAsync(dto.BookId);
            if (book is null)
            {
                throw new NotFoundException("Book not found.");
            }

            var activeBorrow = await _borrowRepository.GetActiveBorrowAsync(dto.BookId, dto.MemberId);
            if (activeBorrow is null)
            {
                throw new ConflictException("This member does not have an active borrow for this book.");
            }

            if (book.AvailableCopies >= book.TotalCopies)
            {
                throw new ConflictException("Book inventory is already fully returned.");
            }

            activeBorrow.ReturnDate = DateTime.UtcNow;
            activeBorrow.Status = ReturnedStatus;
            book.AvailableCopies++;
            var updatedRecord = await _borrowRepository.UpdateAsync(activeBorrow);
            InvalidateBookCache(dto.BookId);
            return MapToDto(updatedRecord);
        }
        finally
        {
            bookLock.Release();
        }
    }

    private static void ValidateBorrowRequest(int bookId, int memberId)
    {
        ValidateId(bookId, "BookId must be greater than 0.");
        ValidateId(memberId, "MemberId must be greater than 0.");
    }


    private static void ValidateId(int id, string message)
    {
        if (id <= 0)
        {
            throw new BadRequestException(message);
        }
    }

    private void InvalidateBookCache(int bookId)
    {
        _cache.Remove(BookCacheKeys.AllBooks);
        _cache.Remove(BookCacheKeys.ById(bookId));
    }

    private static BorrowRecordResponseDto MapToDto(BorrowRecord record) => new()
    {
        Id = record.Id,
        BookId = record.BookId,
        MemberId = record.MemberId,
        BorrowDate = record.BorrowDate,
        ReturnDate = record.ReturnDate,
        Status = record.Status
    };
}
