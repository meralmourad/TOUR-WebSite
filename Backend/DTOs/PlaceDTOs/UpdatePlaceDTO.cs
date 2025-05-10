namespace Backend.DTOs.PlaceDTOs;

public class UpdatePlaceDTO
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Country { get; set; }
    // public List<ImageDTO>? ImageURL { get; set; }

 
}
