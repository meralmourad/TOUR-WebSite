using Backend.DTOs.BookingDTOs;
using Backend.Models;

namespace Backend.Mappers;

public static class BookingMapper
{
    public static BookingDTO MapToBookingDto(Booking booking)
    {
        return new BookingDTO
        {
            Id = booking.Id,
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            SeatsNumber = booking.SeatsNumber,
            IsApproved = booking.IsApproved,
            PhoneNumber = booking.PhoneNumber,
            Comment = booking.Comment,
            Rating = booking.Rating
        };
    }

    public static IEnumerable<BookingDTO> MapToBookingDtoList(IEnumerable<Booking> bookings)
    {
        return bookings.Select(MapToBookingDto).ToList();
    }
}
