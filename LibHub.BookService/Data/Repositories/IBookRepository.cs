using LibHub.BookService.Domain.Entities;

namespace LibHub.BookService.Data.Repositories;

public interface IBookRepository
{
    Task<List<Book>> SearchAsync(string query);
    Task<Book?> GetByIdAsync(Guid id);
    Task<Copy?> GetCopyByIdAsync(Guid id);
    Task<Book> AddAsync(Book book);
    Task<Copy> UpdateCopyAsync(Copy copy);
}
