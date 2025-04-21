using System;
namespace Backend.Models;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string? PhoneNumber { get; set; } = string.Empty;
    public Place? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // Navigation properties
    public List<Message>? ReceivedMessages { get; set; }
    public List<Message>? SentMessages { get; set; }
    public List<Notification>? ReceivedNotifications { get; set; }
    public List<Notification>? SentNotifications { get; set; }
    public List<Report>? Reports { get; set; }

}
