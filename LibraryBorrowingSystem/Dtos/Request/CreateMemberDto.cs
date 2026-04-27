using System.ComponentModel.DataAnnotations;

namespace LibraryBorrowingSystem.Dtos.Request;

public class CreateMemberDto
{
    [Required]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;
}
