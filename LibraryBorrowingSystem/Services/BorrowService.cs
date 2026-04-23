using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Repositories;

namespace LibraryBorrowingSystem.Services;

// TODO (Issue 4): Implement full business logic and concurrency handling
public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    public BorrowService(
        IBorrowRepository borrowRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync()
    {
        var records = await _borrowRepository.GetAllAsync();
        return records.Select(r => new BorrowRecordResponseDto
        {
            Id = r.Id,
            BookId = r.BookId,
            MemberId = r.MemberId,
            BorrowDate = r.BorrowDate,
            ReturnDate = r.ReturnDate,
            Status = r.Status
        });
    }

    public async Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId)
    {
        var records = await _borrowRepository.GetByMemberIdAsync(memberId);
        return records.Select(r => new BorrowRecordResponseDto
        {
            Id = r.Id,
            BookId = r.BookId,
            MemberId = r.MemberId,
            BorrowDate = r.BorrowDate,
            ReturnDate = r.ReturnDate,
            Status = r.Status
        });
    }

    public Task<(BorrowRecordResponseDto? Record, string? Error)> BorrowBookAsync(BorrowRequestDto dto) =>
        throw new NotImplementedException("Issue 4");

    public Task<(BorrowRecordResponseDto? Record, string? Error)> ReturnBookAsync(ReturnRequestDto dto) =>
        throw new NotImplementedException("Issue 4");
}
