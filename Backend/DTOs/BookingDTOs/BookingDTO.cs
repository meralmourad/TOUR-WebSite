using Backend.Models;

namespace Backend.DTOs.BookingDTOs;

public class BookingDTO
{
    public int Id { get; set; }
    public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public required int IsApproved { get; set; }    
    public required int SeatsNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    public string? Description { get; set; } // Ensure this is included if used in the service
}
