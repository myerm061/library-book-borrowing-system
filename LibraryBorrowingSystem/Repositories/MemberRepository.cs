using LibraryBorrowingSystem.Data;
using LibraryBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBorrowingSystem.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _context.Members.AsNoTracking().ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Member> AddAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task DeleteAsync(Member member)
    {
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Members.AnyAsync(m => m.Id == id);
    }
}
