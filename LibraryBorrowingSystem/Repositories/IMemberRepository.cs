using LibraryBorrowingSystem.Models;

namespace LibraryBorrowingSystem.Repositories;

public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> AddAsync(Member member);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Member member);
    Task<bool> ExistsAsync(int id);
}
