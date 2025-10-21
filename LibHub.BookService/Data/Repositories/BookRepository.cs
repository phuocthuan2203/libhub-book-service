using Microsoft.EntityFrameworkCore;
using LibHub.BookService.Domain.Entities;

namespace LibHub.BookService.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _context;

    public BookRepository(BookDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> SearchAsync(string query)
    {
        var normalizedQuery = query.ToLower();
        return await _context.Books
            .Include(b => b.Copies)
            .Where(b => b.Title.ToLower().Contains(normalizedQuery) || 
                       b.Author.ToLower().Contains(normalizedQuery))
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Copy?> GetCopyByIdAsync(Guid id)
    {
        return await _context.Copies
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Copy> UpdateCopyAsync(Copy copy)
    {
        _context.Copies.Update(copy);
        await _context.SaveChangesAsync();
        return copy;
    }
}
