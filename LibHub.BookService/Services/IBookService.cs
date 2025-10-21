using LibHub.BookService.DTOs;

namespace LibHub.BookService.Services;

public interface IBookService
{
    Task<List<BookDto>> SearchBooksAsync(string query);
    Task<BookDetailsDto?> GetBookDetailsByIdAsync(Guid id);
    Task<BookDetailsDto> CreateBookAsync(CreateBookRequest request);
    Task<bool> UpdateCopyStatusAsync(Guid copyId, string newStatus);
}
