using Backend.Models;

namespace Backend.DTOs;

public class ReportDto
{
    public int TripId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    // mappers
    public static ReportDto FromReport(Report report)
    {
        return new ReportDto
        {
            TripId = report.TripId,
            SenderId = report.SenderId,
            Content = report.Content,
            CreatedAt = report.CreatedAt
        };
    }
    public static Report ToReport(ReportDto reportDto)
    {
        return new Report
        {
            TripId = reportDto.TripId,
            SenderId = reportDto.SenderId,
            Content = reportDto.Content,
            CreatedAt = reportDto.CreatedAt
        };
    }
    
}
