using LibHub.BookService.Data.Repositories;
using LibHub.BookService.Domain.Entities;
using LibHub.BookService.DTOs;

namespace LibHub.BookService.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookDto>> SearchBooksAsync(string query)
    {
        var books = await _bookRepository.SearchAsync(query);
        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author
        }).ToList();
    }

    public async Task<BookDetailsDto?> GetBookDetailsByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return null;

        return new BookDetailsDto
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            Description = book.Description,
            Copies = book.Copies.Select(c => new CopyDto
            {
                Id = c.Id,
                Status = c.Status
            }).ToList()
        };
    }

    public async Task<BookDetailsDto> CreateBookAsync(CreateBookRequest request)
    {
        var book = Book.Create(
            request.ISBN,
            request.Title,
            request.Author,
            request.Genre,
            request.Description
        );

        for (int i = 0; i < request.InitialCopyCount; i++)
        {
            book.AddCopy();
        }

        var savedBook = await _bookRepository.AddAsync(book);

        return new BookDetailsDto
        {
            Id = savedBook.Id,
            ISBN = savedBook.ISBN,
            Title = savedBook.Title,
            Author = savedBook.Author,
            Genre = savedBook.Genre,
            Description = savedBook.Description,
            Copies = savedBook.Copies.Select(c => new CopyDto
            {
                Id = c.Id,
                Status = c.Status
            }).ToList()
        };
    }

    public async Task<bool> UpdateCopyStatusAsync(Guid copyId, string newStatus)
    {
        var copy = await _bookRepository.GetCopyByIdAsync(copyId);
        if (copy == null)
            return false;

        copy.UpdateStatus(newStatus);
        await _bookRepository.UpdateCopyAsync(copy);
        return true;
    }
}
