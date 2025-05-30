using System;
using Backend.Data;
using Backend.DTOs.BookingDTOs;
using Backend.IServices;
using Backend.Mappers;
using Backend.Models;
using Backend.WebSockets;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly notificationSocket _nws;
    private readonly Notificationservices _notificationService;
    public BookingService(IUnitOfWork unitOfWork, notificationSocket nws, Notificationservices notificationService)
    {
        _nws = nws;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ApproveBooking(int id, int approved)
    {
        try {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
                Console.WriteLine("\n\n\n\n\n\n in: \n\n\n\n\n\n");    

            if (booking == null){ return false;}
                Console.WriteLine("\n\n\n\n\n\n in: \n\n\n\n\n\n");    

            if (booking.IsApproved == approved) return true;
                Console.WriteLine("\n\n\n\n\n\n in: \n\n\n\n\n\n");    
            
            booking.IsApproved = approved;
            _unitOfWork.BookingRepository.Update(booking);
            
            if (approved == 1) {
                var trip = await _unitOfWork.Trip.GetByIdAsync(booking.TripId);
                if (trip == null) return false;
                if (trip.AvailableSets < booking.SeatsNumber)
                    throw new InvalidOperationException("Not enough available seats");
                
                trip.AvailableSets -= booking.SeatsNumber;
                _unitOfWork.Trip.Update(trip);
                
                var message2 = new {
                    content="Your booking has been approved from trip" + booking.TripId,
                    TripId= booking.TripId
                };
                var messageJson2 = System.Text.Json.JsonSerializer.Serialize(message2);
                await _nws.SendMessageToUserAsync(booking.TouristId, messageJson2);
                var notification2 = new Notification
                {
                    SenderId = booking.TravelAgencyId,
                    Content = "Your booking has been approved from trip",
                };
                await _unitOfWork.Notification.AddAsync(notification2);
                await _unitOfWork.CompleteAsync();
                
            }else if(approved == 0) {
                var trip = await _unitOfWork.Trip.GetByIdAsync(booking.TripId);
                if (trip == null) return false;
                
                trip.AvailableSets += booking.SeatsNumber;
                _unitOfWork.Trip.Update(trip);
            }
            
            await _unitOfWork.CompleteAsync();
            
            var message = new {
                content="Your booking has been approved from trip" + booking.TripId,
                TripId= 0
             };
            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
            await _nws.SendMessageToUserAsync(booking.TouristId, messageJson);

            var notification = new Notification
            {
                SenderId = booking.TravelAgencyId,
                Content = "Your booking has been approved from trip" + booking.TripId,
            };
            await _unitOfWork.Notification.AddAsync(notification);
            await _unitOfWork.CompleteAsync();    
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
        var isBooked = GetBookingsByTouristIdAndTripId(bookingDTO.TouristId, bookingDTO.TripId).Result.Any();
        if (isBooked)
        {
            throw new Exception("You have already booked this trip");
        }
        if (trip.AvailableSets < bookingDTO.SeatsNumber)
            throw new Exception("Not enough available seats");


        var booking = new Booking
        {
            TouristId = bookingDTO.TouristId,
            SeatsNumber = bookingDTO.SeatsNumber,
            TravelAgencyId = bookingDTO.agenceId ?? 0, 
            TripId = bookingDTO.TripId,
            IsApproved = 0,
            PhoneNumber = bookingDTO.PhoneNumber ?? string.Empty 
        };
        await _unitOfWork.BookingRepository.AddAsync(booking);
        await _unitOfWork.CompleteAsync();
    
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

        booking.PhoneNumber = bookingDTO.PhoneNumber ?? booking.PhoneNumber;
        booking.Comment = bookingDTO.Comment ?? booking.Comment;
        booking.Rating = bookingDTO.Rating ?? booking.Rating;

        _unitOfWork.BookingRepository.Update(booking);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<(IEnumerable<BookingDTO> Trips, int TotalCount)> SearchBookingsByQuery(
        int start,
        int len,
        int isApproved,
        bool isAdmin,
        int curid,
        int? USERID,
        int? tripId)
    {
        var bookingsQuery = _unitOfWork.BookingRepository.Query();
        if(isApproved==2)
            bookingsQuery = bookingsQuery.Where(b => b.IsApproved == 0 || b.IsApproved == 1);
        else
            bookingsQuery = bookingsQuery.Where(b => b.IsApproved == isApproved);
        if (USERID.HasValue)
        {
            if(curid != USERID)
                bookingsQuery = bookingsQuery.Where(b => b.Trip.VendorId == curid && b.TouristId == USERID.Value);
            else
                bookingsQuery = bookingsQuery.Where(b => b.Trip.VendorId == USERID.Value || b.TouristId == USERID.Value);
        }
        if (tripId > 0)
        {
            bookingsQuery = bookingsQuery.Where(b => b.TripId == tripId);
        }
        var totalCount = await bookingsQuery.CountAsync();

        var bookings = await Task.Run(() => bookingsQuery
            .Skip(start)
            .Take(len)
            .ToList());

        if (bookings == null || !bookings.Any())
        {
            return (Enumerable.Empty<BookingDTO>(), totalCount);
        }

        var bookingDtos = bookings.Select(b => new BookingDTO
        {
            Id = b.Id,
            TouristId = b.TouristId,
            TripId = b.TripId,
            AgenceId= b.TravelAgencyId,
            IsApproved = b.IsApproved,
            SeatsNumber = b.SeatsNumber,
            PhoneNumber = b.PhoneNumber,
            Comment = b.Comment,
            Rating = b.Rating
        }).ToList();

        return (bookingDtos, totalCount);
    }
}
