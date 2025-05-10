using System;

namespace Backend.Models;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public User? Sender { get; set; } 
    public List<UserNotification>? UserNotifications { get; set; } 
    
}
