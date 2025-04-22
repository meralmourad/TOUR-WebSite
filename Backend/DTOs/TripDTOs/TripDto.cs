using Backend.Models;

namespace Backend.DTOs;

public class TripDto
{
    // Properties of the TripDto class
    public required int Id { get; set; }
    public required int AgenceId { get; set; }  
    public required string Title { get; set; }
    public required int Price { get; set; }
    public required DateTime StartDate { get; set; }
    public string? Description { get; set; }
    public double Rating { get; set; } = 0;
    public required List<int> LocationIds { get; set; }
    public required string[] Images { get; set; }
    public required int Status { get; set; } 
    public required int AvailableSets { get; set; }
    public required List<int> CategoryIds { get; set; }


}
