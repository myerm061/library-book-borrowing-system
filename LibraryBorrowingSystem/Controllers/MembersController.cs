using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBorrowingSystem.Controllers;

// TODO (Issue 3): Add full validation and error handling (DONE JUST NEED TO TEST)
[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponseDto>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberResponseDto>> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);
        if (member is null) return NotFound(new ErrorResponse("Member not found."));
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDto>> Create([FromBody] CreateMemberDto dto)
    {
        try
        {
            var created = await _memberService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MemberResponseDto>> Update(int id, [FromBody] UpdateMemberDto dto)
    {
        try
        {
            var updated = await _memberService.UpdateAsync(id, dto);

            if (updated is null)
                return NotFound(new ErrorResponse("Member not found."));

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _memberService.DeleteAsync(id);
        if (!deleted) return NotFound(new ErrorResponse("Member not found."));
        return NoContent();
    }
}
