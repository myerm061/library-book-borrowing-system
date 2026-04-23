namespace LibraryBorrowingSystem.Dtos.Response;

public class ErrorResponse
{
    public ErrorResponse(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
