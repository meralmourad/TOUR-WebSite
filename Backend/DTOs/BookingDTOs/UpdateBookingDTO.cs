namespace Backend.DTOs.BookingDTOs;

public record class UpdateBookingDTO
{
    public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public bool? IsApproved { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    
    
}
