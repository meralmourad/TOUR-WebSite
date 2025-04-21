using Backend.Models;

namespace Backend.DTOs;

public class TripDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Price { get; set; }
    public DateTime StartDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    // mappers
    public static TripDto FromTrip(Trip trip)
    {
        return new TripDto
        {
            Id = trip.Id,
            Title = trip.Title,
            Price = trip.Price,
            StartDate = trip.StartDate,
            Description = trip.Description,
            Rating = trip.Rating
        };
    }
    public static Trip ToTrip(TripDto tripDto)
    {
        return new Trip
        {
            Id = tripDto.Id,
            Title = tripDto.Title,
            Price = tripDto.Price,
            StartDate = tripDto.StartDate,
            Description = tripDto.Description,
            Rating = tripDto.Rating
        };
    }
    
}
