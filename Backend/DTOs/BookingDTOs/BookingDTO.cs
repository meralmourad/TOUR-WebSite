using Backend.Models;

namespace Backend.DTOs.BookingDTOs;

public class BookingDTO
{
   public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public required bool IsApproved { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    //Mappers
    public static BookingDTO ToDTO(Booking booking)
    {
        return new BookingDTO
        {
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            IsApproved = booking.IsApproved,
            PhoneNumber = booking.PhoneNumber,
            Comment = booking.Comment,
            Rating = booking.Rating
        };
    }
    public static Booking TODB(BookingDTO bookingDTO)
    {
        return new Booking
        {
            TouristId = bookingDTO.TouristId,
            TripId = bookingDTO.TripId,
            IsApproved = bookingDTO.IsApproved,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty,
            Comment = bookingDTO.Comment ?? string.Empty,
            Rating = bookingDTO.Rating ?? 0
        };
    }
}
