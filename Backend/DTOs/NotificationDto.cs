using Backend.Models;

namespace Backend.DTOs;

public class NotificationDto
{
    public  int Id { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required string Context { get; set; }
    
}
