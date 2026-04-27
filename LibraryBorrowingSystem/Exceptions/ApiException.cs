// ApiException.cs

using Microsoft.AspNetCore.Http;

// namespace(s)
namespace LibraryBorrowingSystem.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

public sealed class BadRequestException : ApiException
{
    public BadRequestException(string message)
        : base(message, StatusCodes.Status400BadRequest){}
}

public sealed class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(message, StatusCodes.Status404NotFound) {}
}

public sealed class ConflictException : ApiException
{
    public ConflictException(string message)
        : base(message, StatusCodes.Status409Conflict){}
}
