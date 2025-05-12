using System;
using Backend.Data;
using Backend.DTOs.BookingDTOs;
using Backend.IServices;
using Backend.Mappers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaProducerService kafkaProducerService;
    public BookingService(IUnitOfWork unitOfWork, IKafkaProducerService kafkaProducerService)
    {
        _unitOfWork = unitOfWork;
        this.kafkaProducerService = kafkaProducerService;
    }

    public async Task<bool> ApproveBooking(int id, int approved)
    {
        try {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (booking == null) return false;
            
            if (booking.IsApproved == approved) return true;
            
            booking.IsApproved = approved;
            _unitOfWork.BookingRepository.Update(booking);
            
            if (approved == 1) {
                var trip = await _unitOfWork.Trip.GetByIdAsync(booking.TripId);
                if (trip == null) return false;
                
                if (trip.AvailableSets < booking.SeatsNumber)
                    throw new InvalidOperationException("Not enough available seats");
                    
                trip.AvailableSets -= booking.SeatsNumber;
                _unitOfWork.Trip.Update(trip);
            }
            
            await _unitOfWork.CompleteAsync();
            
            var message = new { BookingId = booking.Id, TripId = booking.TripId, NumOfSeats = booking.SeatsNumber };
            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
            // await kafkaProducerService.ProduceAsync("booking-events", messageJson);
            
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine($"Error approving booking: {ex.Message}");
            return false;
        }
    }

    public async Task<BookingDTO> CreateBooking(CreateBookingDto bookingDTO)
    {
        var agenceId = _unitOfWork.Trip.Query()
            .Where(t => t.Id == bookingDTO.TripId)
            .Select(t => t.VendorId)
            .FirstOrDefault();
        bookingDTO.agenceId = agenceId;
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
        // check if the trip has enough available seats
        if (trip.AvailableSets < bookingDTO.SeatsNumber)
            throw new Exception("Not enough available seats");


        var booking = new Booking
        {
            TouristId = bookingDTO.TouristId,
            SeatsNumber = bookingDTO.SeatsNumber,
            TravelAgencyId = bookingDTO.agenceId ?? 0, // Default to 0 if null
            TripId = bookingDTO.TripId,
            IsApproved = 0,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty // Default to empty string if null
        };
        await _unitOfWork.BookingRepository.AddAsync(booking);
        await _unitOfWork.CompleteAsync();
        // Booking dto
    
        var bookingDto = new BookingDTO
        {
            TouristId = booking.TouristId,
            SeatsNumber = bookingDTO.SeatsNumber,
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
        return BookingMapper.MapToBookingDtoList(bookings).ToList();
    }

    public async Task<BookingDTO?> GetBookingById(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        return booking == null ? null : BookingMapper.MapToBookingDto(booking);
    }

    public Task<List<BookingDTO>> GetBookingsByAgencyId(int agencyId)
    {
        var bookings = _unitOfWork.BookingRepository.Query()
            .Where(b => b.Trip.VendorId == agencyId)
            .ToList();
        return Task.FromResult(BookingMapper.MapToBookingDtoList(bookings).ToList());
    }

    public async Task<List<BookingDTO>> GetBookingsByTouristId(int touristId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TouristId == touristId)
            .ToList();
        return BookingMapper.MapToBookingDtoList(bookings).ToList();
    }

    public async Task<List<BookingDTO>> GetBookingsByTouristIdAndTripId(int touristId, int tripId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TouristId == touristId && b.TripId == tripId)
            .ToList();
        return BookingMapper.MapToBookingDtoList(bookings).ToList();
    }

    public async Task<List<BookingDTO>> GetBookingsByTripId(int tripId)
    {
        var bookings = (await _unitOfWork.BookingRepository.GetAllAsync())
            .Where(b => b.TripId == tripId)
            .ToList();
        return BookingMapper.MapToBookingDtoList(bookings).ToList();
    }

    public Task<int> RateBooking(int id, int rating, string? comment = null)
    {
        var booking = _unitOfWork.BookingRepository.GetByIdAsync(id).Result;
        if (booking == null)
            return Task.FromResult(0);
        booking.Rating = rating;
        booking.Comment = comment ?? booking.Comment;
        _unitOfWork.BookingRepository.Update(booking);


        // calc the average rating of the trip
        var trip = _unitOfWork.Trip.GetByIdAsync(booking.TripId).Result;
        if (trip == null)
            return Task.FromResult(0);
        var bookings = _unitOfWork.BookingRepository.Query()
            .Where(b => b.TripId == trip.Id)
            .ToList();
        var avgRating = bookings.Average(b => b.Rating);
        trip.Rating = (int)avgRating;
        _unitOfWork.Trip.Update(trip);

        _unitOfWork.CompleteAsync();
        return Task.FromResult(rating);
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

    public async Task<List<BookingDTO>> SearchBookingsByQuery(
        int start,
        int len,
        bool isApproved,
        bool isAdmin,
        int? USERID,
        int? tripId)
    {
        // Filter bookings based on the provided parameters
        var bookingsQuery = _unitOfWork.BookingRepository.Query();

        // Apply filters
        int isApprovedInt = isApproved ? 1 : 0;
        bookingsQuery = bookingsQuery.Where(b => b.IsApproved == isApprovedInt);
        if (USERID.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.Trip.VendorId == USERID.Value || b.TouristId == USERID.Value);
        }
        if (tripId > 0)
        {
            bookingsQuery = bookingsQuery.Where(b => b.TripId == tripId);
        }
        // Materialize the query to avoid serialization issues
        var bookings = bookingsQuery
            .Skip(start)
            .Take(len)
            .ToList();

        // Map to DTOs
        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            Id = b.Id,
            TouristId = b.TouristId,
            TripId = b.TripId,
            IsApproved = b.IsApproved,
            SeatsNumber = b.SeatsNumber,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();

        return bookingDtos;
    }
}
