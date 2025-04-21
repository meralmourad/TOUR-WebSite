namespace Backend.DTOs.BookingDTOs;

public record class UpdateBookingDTO
{
    public required int TouristId { get; set; }
    public required int TripId { get; set; }
    public bool? IsApproved { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    //Mappers
    public static UpdateBookingDTO ToDTO(Models.Booking booking)
    {
        return new UpdateBookingDTO
        {
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            IsApproved = booking.IsApproved,
            PhoneNumber = booking.PhoneNumber,
            Comment = booking.Comment,
            Rating = booking.Rating
        };
    }
    public static Models.Booking ToDB(UpdateBookingDTO bookingDTO)
    {
        return new Models.Booking
        {
            TouristId = bookingDTO.TouristId,
            TripId = bookingDTO.TripId,
            IsApproved = bookingDTO.IsApproved ?? false,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty,
            Comment = bookingDTO.Comment ?? string.Empty,
            Rating = bookingDTO.Rating ?? 0
        };
    }
    public static Models.Booking ToDB(Models.Booking booking, UpdateBookingDTO bookingDTO)
    {
        booking.IsApproved = bookingDTO.IsApproved ?? booking.IsApproved;
        booking.PhoneNumber = bookingDTO.PhoneNumber ?? booking.PhoneNumber;
        booking.Comment = bookingDTO.Comment ?? booking.Comment;
        booking.Rating = bookingDTO.Rating ?? booking.Rating;
        return booking;
    }

}
