using System;
using Backend.DTOs.BookingDTOs;
using Backend.Models;

namespace Backend.IServices;

public interface IBookingService
{
    Task<BookingDTO> CreateBooking(CreateBookingDto bookingDTO);
    Task<bool> UpdateBooking(int id, UpdateBookingDTO bookingDTO);
    Task<bool> DeleteBooking(int id);
    Task<BookingDTO?> GetBookingById(int id);
    Task<List<BookingDTO>> GetAllBookings();
    Task<List<BookingDTO>> GetBookingsByTouristId(int touristId);
    Task<List<BookingDTO>> GetBookingsByTripId(int tripId);
    Task<List<BookingDTO>> GetBookingsByTouristIdAndTripId(int touristId, int tripId);
    Task<List<BookingDTO>> GetBookingsByAgencyId(int agencyId);
    Task<bool> ApproveBooking(int id,int Approved);
    Task<int> RateBooking(int id, int rating, string? comment = null);
    Task<(IEnumerable<BookingDTO> Trips, int TotalCount)> SearchBookingsByQuery(
        int start,
        int len,
        int isApproved,
        bool isAdmin,
        int curid,
        int? agencyId,
        int? tripId);
    // Task GetBookingsByTouristId(string? userId);
}
