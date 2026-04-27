using System.ComponentModel.DataAnnotations;

namespace LibraryBorrowingSystem.Dtos.Request;

public class ReturnRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be greater than 0.")]
    public int BookId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MemberId must be greater than 0.")]
    public int MemberId { get; set; }
}
