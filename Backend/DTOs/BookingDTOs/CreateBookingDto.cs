namespace Backend.DTOs.BookingDTOs;

public class CreateBookingDto
{
    public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public string? PhoneNumber { get; set; }
    //mappers
    public static Models.Booking ToDB(CreateBookingDto bookingDTO)
    {
        return new Models.Booking
        {
            TouristId = bookingDTO.TouristId,
            TripId = bookingDTO.TripId,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty,
            Comment = string.Empty,
            IsApproved = false,
            Rating = 0
        };
    }
    public static CreateBookingDto ToDTO(Models.Booking booking)
    {
        return new CreateBookingDto
        {
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            PhoneNumber = booking.PhoneNumber ?? string.Empty
        };
    }

}
