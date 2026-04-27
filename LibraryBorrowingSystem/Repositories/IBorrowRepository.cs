using LibraryBorrowingSystem.Models;

namespace LibraryBorrowingSystem.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(int memberId);
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<BorrowRecord?> GetActiveBorrowAsync(int bookId, int memberId);
    Task<BorrowRecord> AddAsync(BorrowRecord record);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);
}
