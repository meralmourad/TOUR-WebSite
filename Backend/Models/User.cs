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
    public string? Address { get; set; }
    
    // Navigation properties
    public List<Message>? ReceivedMessages { get; set; }
    public List<Message>? SentMessages { get; set; }
    public List<Report>? Reports { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }
    public ICollection<Notification> SentNotifications { get; set; }
    public List<Booking>? Bookings { get; set; }

}
