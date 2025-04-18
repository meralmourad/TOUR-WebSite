using System;

namespace Backend.Models;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public int ReceiverId { get; set; } 
    public User? Receiver { get; set; } 

    public int SenderId { get; set; }
    public User? Sender { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
