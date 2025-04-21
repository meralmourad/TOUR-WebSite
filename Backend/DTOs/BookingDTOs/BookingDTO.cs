namespace Backend.DTOs.BookingDTOs;

public record class BookingDTO
{
   public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public required bool IsApproved { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
}
