namespace Backend.DTOs.PlaceDTOs;

public class GetPlaceDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Country { get; set; }
    // public required List<ImageDTO> ImageURL { get; set; }

}
