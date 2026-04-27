using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;

namespace LibraryBorrowingSystem.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberResponseDto>> GetAllAsync();
    Task<MemberResponseDto?> GetByIdAsync(int id);
    Task<MemberResponseDto> CreateAsync(CreateMemberDto dto);
    Task<MemberResponseDto?> UpdateAsync(int id, UpdateMemberDto dto);
    Task<bool> DeleteAsync(int id);
}
