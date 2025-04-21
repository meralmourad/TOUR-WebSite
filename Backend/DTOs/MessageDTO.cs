namespace Backend.DTOs;

public record class MessageDTO
{
    public required int Id { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required string Content { get; set; }
    public required DateTime SentAt { get; set; }
}
