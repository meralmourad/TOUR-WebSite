namespace Backend.DTOs.TripDTOs;

public record class CreateTripDTO
{
    public required int AgenceId { get; set; }  
    public required string Title { get; set; }
    public required int Price { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public string? Description { get; set; }
    //public double Rating { get; set; } = 0;
    public required List<int> LocationIds { get; set; }
    public string[]? Images { get; set; }
    // public int Status { get; set; } 
    public required int AvailableSets { get; set; }
    public required List<int> CategoryIds { get; set; }
    //--------
}
