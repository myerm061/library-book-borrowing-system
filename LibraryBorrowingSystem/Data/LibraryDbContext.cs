using LibraryBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBorrowingSystem.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Member)
                .WithMany()
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
