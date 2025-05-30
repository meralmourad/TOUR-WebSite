using Backend.Models;

namespace Backend.DTOs;

public class ReportDto
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
}
