using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBorrowingSystem.Controllers;

// TODO (Issue 4): Add concurrency handling and full validation
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
        var (record, error) = await _borrowService.BorrowBookAsync(dto);
        if (error is not null) return BadRequest(new ErrorResponse(error));
        return CreatedAtAction(nameof(GetAll), record);
    }

    [HttpPost("return")]
    public async Task<ActionResult<BorrowRecordResponseDto>> Return([FromBody] ReturnRequestDto dto)
    {
        var (record, error) = await _borrowService.ReturnBookAsync(dto);
        if (error is not null) return BadRequest(new ErrorResponse(error));
        return Ok(record);
    }
}
