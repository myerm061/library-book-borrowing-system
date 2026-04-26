namespace LibraryBorrowingSystem.Services;

internal static class BookCacheKeys
{
    public const string AllBooks = "books_all";

    public static string ById(int id) => $"book_{id}";
}
