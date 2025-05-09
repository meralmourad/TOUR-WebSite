namespace Backend.DTOs.PlaceDTOs;

public class CreatePlaceDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Country { get; set; }
    // public required string[] ImageURL { get; set; }
}
