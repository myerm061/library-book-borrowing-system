using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Repositories;

namespace LibraryBorrowingSystem.Services;

// TODO (Issue 3): Implement full business logic
public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MemberResponseDto>> GetAllAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(m => new MemberResponseDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            MembershipDate = m.MembershipDate
        });
    }

    public async Task<MemberResponseDto?> GetByIdAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member is null) return null;
        return new MemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
    }

    public Task<MemberResponseDto> CreateAsync(CreateMemberDto dto) => throw new NotImplementedException("Issue 3");
    public Task<MemberResponseDto?> UpdateAsync(int id, UpdateMemberDto dto) => throw new NotImplementedException("Issue 3");
    public Task<bool> DeleteAsync(int id) => throw new NotImplementedException("Issue 3");
}
