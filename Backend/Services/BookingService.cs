using System;
using Backend.Data;
using Backend.DTOs.BookingDTOs;
using Backend.IServices;
using Backend.Models;

namespace Backend.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BookingDTO> CreateBooking(CreateBookingDto bookingDTO)
    {
        var user = _unitOfWork.User.GetByIdAsync(bookingDTO.TouristId).Result
             ?? throw new Exception("User not found");
        var trip = _unitOfWork.Trip.GetByIdAsync(bookingDTO.TripId).Result 
            ?? throw new Exception("Trip not found");
        //check if the user has already booked the trip
        var isBooked = GetBookingsByTouristIdAndTripId(bookingDTO.TouristId, bookingDTO.TripId).Result.Any();
        if (isBooked)
        {
            throw new Exception("You have already booked this trip");
        }
        var booking = new Booking
        {
            TouristId = bookingDTO.TouristId,
            TripId = bookingDTO.TripId,
            IsApproved = false,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty
        };
        await _unitOfWork.BookingRepository.AddAsync(booking);
        await _unitOfWork.CompleteAsync();
        // Booking dto
        var bookingDto = new BookingDTO
        {
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            IsApproved = booking.IsApproved,
            PhoneNumber = booking.PhoneNumber,
            Comment = booking.Comment,
            Rating = booking.Rating
        };
        return bookingDto;
    }

    public async Task<bool> DeleteBooking(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return false;
        }
        _unitOfWork.BookingRepository.Delete(booking);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<List<BookingDTO>> GetAllBookings()
    {
        var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
        // Correct DTO mapping
        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            TouristId = b.TouristId,
            TripId = b.TripId,
            IsApproved = b.IsApproved,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();
        return bookingDtos;
    }

    public async Task<BookingDTO?> GetBookingById(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return null;
        }
        // Correct DTO mapping
        var bookingDto = new BookingDTO
        {
            TouristId = booking.TouristId,
            TripId = booking.TripId,
            IsApproved = booking.IsApproved,
            PhoneNumber = booking.PhoneNumber,
            Comment = booking.Comment,
            Rating = booking.Rating
        };
        return bookingDto;
    }

    public async Task<List<BookingDTO>> GetBookingsByTouristId(int touristId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TouristId == touristId)
            .ToList();
        // Correct DTO mapping
        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            TouristId = b.TouristId,
            TripId = b.TripId,
            IsApproved = b.IsApproved,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();
        return bookingDtos;
    }

    public async Task<List<BookingDTO>> GetBookingsByTouristIdAndTripId(int touristId, int tripId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TouristId == touristId && b.TripId == tripId)
            .ToList();
        // Correct DTO mapping
        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            TouristId = b.TouristId,
            TripId = b.TripId,
            IsApproved = b.IsApproved,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();
        return bookingDtos;
    }

    public async Task<List<BookingDTO>> GetBookingsByTripId(int tripId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TripId == tripId)
            .ToList();
        // Correct DTO mapping
        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            TouristId = b.TouristId,
            TripId = b.TripId,
            IsApproved = b.IsApproved,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();
        return bookingDtos;
    }

    public async Task<bool> UpdateBooking(int id, UpdateBookingDTO bookingDTO)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
            return false;

        booking.IsApproved = bookingDTO.IsApproved ?? booking.IsApproved;
        booking.PhoneNumber = bookingDTO.PhoneNumber ?? booking.PhoneNumber;
        booking.Comment = bookingDTO.Comment ?? booking.Comment;
        booking.Rating = bookingDTO.Rating ?? booking.Rating;

        _unitOfWork.BookingRepository.Update(booking);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
