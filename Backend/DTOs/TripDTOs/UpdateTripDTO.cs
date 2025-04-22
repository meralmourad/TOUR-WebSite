namespace Backend.DTOs.TripDTOs;

public record class UpdateTripDTO
{
    public string? Title { get; set; }
    public int? Price { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Description { get; set; }
    public double? Rating { get; set; } = 0;
    public List<int>? LocationIds { get; set; }
    public string[]? Images { get; set; }
    public int? Status { get; set; } 
    public int? AvailableSets { get; set; }
    public List<int>? CategoryIds { get; set; }
}
