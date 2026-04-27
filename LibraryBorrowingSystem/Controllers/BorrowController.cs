// BorrowConstrollers.cs

using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

// namespace(s)
namespace LibraryBorrowingSystem.Controllers;


[ApiController]
[Route("api/borrow")]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _borrowService;
    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BorrowRecordResponseDto>>> GetAll()
    {
        var records = await _borrowService.GetAllAsync();
        return Ok(records);
    }

    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<IEnumerable<BorrowRecordResponseDto>>> GetByMember(int memberId)
    {
        var records = await _borrowService.GetByMemberIdAsync(memberId);
        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<BorrowRecordResponseDto>> Borrow([FromBody] BorrowRequestDto dto)
    {
        var record = await _borrowService.BorrowBookAsync(dto);
        return StatusCode(StatusCodes.Status201Created, record);
    }

    [HttpPost("return")]
    public async Task<ActionResult<BorrowRecordResponseDto>> Return([FromBody] ReturnRequestDto dto)
    {
        var record = await _borrowService.ReturnBookAsync(dto);
        return Ok(record);
    }
}// end of class
