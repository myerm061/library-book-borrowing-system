// IBorrowService.cs

using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;

namespace LibraryBorrowingSystem.Services;

public interface IBorrowService
{
    Task<IEnumerable<BorrowRecordResponseDto>> GetAllAsync();
    Task<IEnumerable<BorrowRecordResponseDto>> GetByMemberIdAsync(int memberId);
    Task<BorrowRecordResponseDto> BorrowBookAsync(BorrowRequestDto dto);
    Task<BorrowRecordResponseDto> ReturnBookAsync(ReturnRequestDto dto);
}
