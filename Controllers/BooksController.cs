using LibraryBorrowingSystem.Dtos.Request;
using LibraryBorrowingSystem.Dtos.Response;
using LibraryBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBorrowingSystem.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null) return NotFound(new ErrorResponse("Book not found."));
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create([FromBody] CreateBookDto dto)
    {
        try
        {
            var created = await _bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, [FromBody] UpdateBookDto dto)
    {
        try
        {
            var updated = await _bookService.UpdateAsync(id, dto);
            if (updated is null) return NotFound(new ErrorResponse("Book not found."));
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
        var deleted = await _bookService.DeleteAsync(id);
        if (!deleted) return NotFound(new ErrorResponse("Book not found."));
        return NoContent();
    }
}
