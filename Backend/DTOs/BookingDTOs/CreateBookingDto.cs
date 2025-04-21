namespace Backend.DTOs.BookingDTOs;

public record class CreateBookingDto
{
    public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public string? PhoneNumber { get; set; }

}
