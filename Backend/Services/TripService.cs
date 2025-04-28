using System;
using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Backend.Models;

namespace Backend.Services;

public class TripService : ITripService
{
    private readonly IUnitOfWork _unitOfWork;
    public TripService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private void ThrowIfErrorFound(bool condition, string errorMessage)
    {
        if (condition)
            throw new Exception(errorMessage);
    }

    public async Task<(bool Success, string Message)> CreateTripAsync(CreateTripDTO tripDto)
    {
        ThrowIfErrorFound(tripDto.Price <= 0, "Price must be greater than zero.");

        var trip = new Trip
        {
            VendorId = tripDto.AgenceId,
            Title = tripDto.Title,
            Price = tripDto.Price,
            StartDate = tripDto.StartDate,
            Description = tripDto.Description ?? string.Empty,
            Rating = 0,
            LocationIds = tripDto.LocationIds ?? new List<int>(),
            Images = tripDto.Images ?? Array.Empty<string>(),
            Status =  0,
            AvailableSets = tripDto.AvailableSets,
            CategoryIds = tripDto.CategoryIds ?? new List<int>(),
        };
        await _unitOfWork.Trip.AddAsync(trip);
        await _unitOfWork.CompleteAsync();
        return (true, "Trip created successfully.");    
    }

    public async Task<(bool Success, string Message)> DeleteTripAsync(int id)
    {
        var trip = await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(trip == null, "Trip not found.");

        _unitOfWork.Trip.Delete(trip);
        await _unitOfWork.CompleteAsync();
        return (true, "Trip deleted successfully.");
    }
    public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
    {
        var trips = await _unitOfWork.Trip.GetAllAsync();
        ThrowIfErrorFound(trips == null || !trips.Any(), "No trips found.");

        return trips.Select(t => new TripDto
        {
            Id = t.Id,
            AgenceId = t.VendorId,
            Title = t.Title,
            Price = t.Price,
            StartDate = t.StartDate,
            Description = t.Description,
            Rating = t.Rating,
            LocationIds = t.LocationIds,
            Images = t.Images,
            Status = t.Status,
            AvailableSets = t.AvailableSets,
            CategoryIds = t.CategoryIds
        }).ToList();
    }

    public async Task<TripDto?> GetTripByIdAsync(int id)
    {
        var trip =await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(trip == null, "Trip not found.");

        return new TripDto
        {
            Id = trip.Id,
            AgenceId = trip.VendorId,
            Title = trip.Title,
            Price = trip.Price,
            StartDate = trip.StartDate,
            Description = trip.Description,
            Rating = trip.Rating,
            LocationIds = trip.LocationIds,
            Images = trip.Images,
            Status = trip.Status,
            AvailableSets = trip.AvailableSets,
            CategoryIds = trip.CategoryIds
        };
    }

    public async Task<(bool Success, string Message)> UpdateTripAsync(int id, UpdateTripDTO tripDto)
    {
        var curTrip = await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(curTrip == null, "Trip not found.");

        curTrip.Title = tripDto.Title ?? curTrip.Title;
        curTrip.Price = tripDto.Price ?? curTrip.Price;
        curTrip.StartDate = tripDto.StartDate ?? curTrip.StartDate;
        curTrip.Description = tripDto.Description ?? curTrip.Description;
        curTrip.Rating = tripDto.Rating ?? curTrip.Rating;
        curTrip.LocationIds = tripDto.LocationIds ?? curTrip.LocationIds;
        curTrip.Images = tripDto.Images ?? curTrip.Images;
        curTrip.Status = tripDto.Status ?? curTrip.Status;
        curTrip.AvailableSets = tripDto.AvailableSets ?? curTrip.AvailableSets;
        curTrip.CategoryIds = tripDto.CategoryIds ?? curTrip.CategoryIds;

        _unitOfWork.Trip.Update(curTrip);
        await _unitOfWork.CompleteAsync();
        return (true, "Trip updated successfully.");
    }

    public async Task<IEnumerable<Trip>> SearchTripsAsync(int start, int len, string? destination, DateTime? startDate)
    {
        // Example implementation
        var trips = (await _unitOfWork.Trip.GetAllAsync()).AsQueryable();

        if (!string.IsNullOrEmpty(destination))
        {
            trips = trips.Where(t => t.Description.Contains(destination));
        }

        if (startDate.HasValue)
        {
            trips = trips.Where(t => t.StartDate >= startDate.Value);
        }

        return trips.Skip(start).Take(len).ToList();
    }

    public IEnumerable<Trip> SearchTripsByQuery(string? query, int start, int len, string? destination, DateTime? startDate)
    {
        var tripsQuery = _unitOfWork.Trip.GetAllAsync().Result.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            tripsQuery = tripsQuery.Where(t => t.Title.Contains(query) || t.Description.Contains(query));
        }

        if (!string.IsNullOrEmpty(destination))
        {
            tripsQuery = tripsQuery.Where(t => t.Description.Contains(destination));
        }

        if (startDate.HasValue)
        {
            tripsQuery = tripsQuery.Where(t => t.StartDate >= startDate.Value);
        }

        return tripsQuery.Skip(start).Take(len).ToList();
    }
}
