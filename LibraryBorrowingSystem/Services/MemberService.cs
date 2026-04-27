using System.ComponentModel.DataAnnotations;
using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Repositories;
using LibraryBorrowingSystem.Models;

namespace LibraryBorrowingSystem.Services;

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
    public async Task<MemberResponseDto> CreateAsync(CreateMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required.");

        if (!new EmailAddressAttribute().IsValid(dto.Email))
            throw new ArgumentException("Email must be valid.");

        var member = new Member
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            MembershipDate = DateTime.UtcNow
        };

        var created = await _memberRepository.AddAsync(member);

        return new MemberResponseDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            MembershipDate = created.MembershipDate
        };
    }

    public async Task<MemberResponseDto?> UpdateAsync(int id, UpdateMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required.");

        if (!new EmailAddressAttribute().IsValid(dto.Email))
            throw new ArgumentException("Email must be valid.");

        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
            return null;

        member.FullName = dto.FullName.Trim();
        member.Email = dto.Email.Trim();

        var updated = await _memberRepository.UpdateAsync(member);

        return new MemberResponseDto
        {
            Id = updated.Id,
            FullName = updated.FullName,
            Email = updated.Email,
            MembershipDate = updated.MembershipDate
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
            return false;

        await _memberRepository.DeleteAsync(member);
        return true;
    }
}
