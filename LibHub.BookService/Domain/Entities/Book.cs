namespace LibHub.BookService.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string ISBN { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Genre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private readonly List<Copy> _copies = new();
    public IReadOnlyList<Copy> Copies => _copies.AsReadOnly();

    private Book() 
    {
        CreatedAt = DateTime.UtcNow;
    }

    public static Book Create(string isbn, string title, string author, string genre, string description)
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            ISBN = isbn,
            Title = title,
            Author = author,
            Genre = genre,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddCopy()
    {
        var copy = Copy.Create(Id);
        _copies.Add(copy);
    }
}
