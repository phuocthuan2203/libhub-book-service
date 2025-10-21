namespace LibHub.BookService.Domain.Entities;

public class Copy
{
    public Guid Id { get; private set; }
    public string Status { get; private set; } = "Available";
    public Guid BookId { get; private set; }
    public Book? Book { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Copy() 
    {
        CreatedAt = DateTime.UtcNow;
    }

    public static Copy Create(Guid bookId)
    {
        return new Copy
        {
            Id = Guid.NewGuid(),
            BookId = bookId,
            Status = "Available",
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty", nameof(newStatus));

        var validStatuses = new[] { "Available", "On Loan", "Damaged", "Lost" };
        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException($"Invalid status. Must be one of: {string.Join(", ", validStatuses)}", nameof(newStatus));

        Status = newStatus;
    }
}
