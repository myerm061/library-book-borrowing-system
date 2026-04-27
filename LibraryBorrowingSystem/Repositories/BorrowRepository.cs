using LibraryBorrowingSystem.Data;
using LibraryBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBorrowingSystem.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly LibraryDbContext _context;

    public BorrowRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        return await _context.BorrowRecords.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(int memberId)
    {
        return await _context.BorrowRecords
            .AsNoTracking()
            .Where(r => r.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<BorrowRecord?> GetByIdAsync(int id)
    {
        return await _context.BorrowRecords.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<BorrowRecord?> GetActiveBorrowAsync(int bookId, int memberId)
    {
        return await _context.BorrowRecords
            .FirstOrDefaultAsync(r => r.BookId == bookId && r.MemberId == memberId && r.Status == "Borrowed");
    }

    public async Task<BorrowRecord> AddAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<BorrowRecord> UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
        return record;
    }
}
