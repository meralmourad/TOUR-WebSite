namespace Backend.DTOs.BookingDTOs;

public record class UpdateBookingDTO
{
    public  int TouristId { get; set; }
    public  int TripId { get; set; }
    public int? SeatsNumber { get; set; }

    // public int? IsApproved { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    
    
}
