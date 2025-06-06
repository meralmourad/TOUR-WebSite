namespace Backend.DTOs.BookingDTOs;

public class CreateBookingDto
{
    public int Id { get; set; }
    public  int TouristId { get; set; }
    public required int TripId { get; set; }
    public int? agenceId { get; set; }
    public required int SeatsNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public int IsApproved { get; set; } = 0;
}
