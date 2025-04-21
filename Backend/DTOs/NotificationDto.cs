namespace Backend.DTOs;

public class NotificationDto
{
    public required int Id { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required string Context { get; set; }
    public  required DateTime CreatedAt { get; set; }
}
