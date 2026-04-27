using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;

namespace LibraryBorrowingSystem.Services;

public interface IBorrowService
{
    Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync();
    Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId);
    Task<(BorrowRecordResponseDto? Record, string? Error)> BorrowBookAsync(BorrowRequestDto dto);
    Task<(BorrowRecordResponseDto? Record, string? Error)> ReturnBookAsync(ReturnRequestDto dto);
}
