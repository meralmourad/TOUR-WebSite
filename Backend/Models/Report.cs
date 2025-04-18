using System;

namespace Backend.Models;

public class Report
{
    public int TripId { get; set; } 
    public Trip? Trip { get; set; }
    public int SenderId { get; set; }
    public User? Sender { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
