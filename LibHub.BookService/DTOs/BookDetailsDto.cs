namespace LibHub.BookService.DTOs;

public class BookDetailsDto
{
    public Guid Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CopyDto> Copies { get; set; } = new();
}
