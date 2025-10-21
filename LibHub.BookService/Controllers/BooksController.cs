using Microsoft.AspNetCore.Mvc;
using LibHub.BookService.DTOs;
using LibHub.BookService.Services;

namespace LibHub.BookService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var results = await _bookService.SearchBooksAsync(query);
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _bookService.GetBookDetailsByIdAsync(id);
        if (book == null)
            return NotFound($"Book with ID {id} not found");

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var book = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("copies/{id}/status")]
    public async Task<IActionResult> GetCopyStatus(Guid id)
    {
        try
        {
            var status = await _bookService.GetCopyStatusAsync(id);
            if (status == null)
                return NotFound($"Copy with ID {id} not found");

            return Ok(new { status });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("copies/{id}/status")]
    public async Task<IActionResult> UpdateCopyStatus(Guid id, [FromBody] UpdateCopyStatusRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _bookService.UpdateCopyStatusAsync(id, request.Status);
            if (!success)
                return NotFound($"Copy with ID {id} not found");

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
