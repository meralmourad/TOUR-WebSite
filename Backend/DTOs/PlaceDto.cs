using Backend.Models;

namespace Backend.DTOs;

public class PlaceDto
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public required string Country { get; set; }
    public required string[] ImageURL { get; set; }
    //mappers
    public static PlaceDto FromPlace(Place place)
    {
        return new PlaceDto
        {
            Id = place.Id,
            Name = place.Name,
            Description = place.Description,
            Country = place.Country,
            ImageURL = place.ImagesURL
        };
    }
    public static Place ToPlace(PlaceDto placeDto)
    {
        return new Place
        {
            Id = placeDto.Id,
            Name = placeDto.Name,
            Description = placeDto.Description,
            Country = placeDto.Country,
            ImagesURL = placeDto.ImageURL
        };
    }
}
