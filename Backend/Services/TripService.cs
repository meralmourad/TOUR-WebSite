using System;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.PlaceDTOs;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Backend.Mappers;
using Backend.Models;

namespace Backend.Services;

public class TripService : ITripService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TripMapper _tripMapper;

    public TripService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _tripMapper = new TripMapper(unitOfWork);
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
            Status =  0,
            AvailableSets = tripDto.AvailableSets,
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
        return _tripMapper.MapToTripDtoList(trips);
    }

    public async Task<TripDto?> GetTripByIdAsync(int id)
    {
        var trip = await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(trip == null, "Trip not found.");
        return _tripMapper.MapToTripDto(trip);
    }

    public async Task<(bool Success, string Message)> UpdateTripAsync(int id, UpdateTripDTO tripDto)
    {
        // var userIdClaim =;
        var curTrip = await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(curTrip == null, "Trip not found.");

        curTrip.Title = tripDto.Title ?? curTrip.Title;
        curTrip.Price = tripDto.Price ?? curTrip.Price;
        curTrip.StartDate = tripDto.StartDate ?? curTrip.StartDate;
        curTrip.Description = tripDto.Description ?? curTrip.Description;
        curTrip.Rating = tripDto.Rating ?? curTrip.Rating;
        curTrip.AvailableSets = tripDto.AvailableSets ?? curTrip.AvailableSets;

        _unitOfWork.Trip.Update(curTrip);
        await _unitOfWork.CompleteAsync();
        return (true, "Trip updated successfully.");
    }

    public (IEnumerable<TripDto> Trips, int TotalCount) SearchTripsByQuery(
        string? q, int start, int len,
        string? destination, DateOnly? startDate,
        DateOnly? endDate, int startPrice, int endPrice,
        bool isApproved, bool isAdmin, int agencyId)
    {
        // 1. Build the base query and filter as much as possible in the DB
        var tripsQuery = _unitOfWork.Trip.Query();

        if (!isApproved)
        {
            if (isAdmin)
                tripsQuery = tripsQuery.Where(t => t.Status == 0);
            else
                tripsQuery = tripsQuery.Where(t => t.Status == 0 && t.VendorId == agencyId);
        }
        else
        {
            tripsQuery = tripsQuery.Where(t => t.Status == 1);
        }

        start = Math.Max(0, start);
        len = Math.Max(1, len);
        startPrice = Math.Max(0, startPrice);
        endPrice = Math.Max(0, endPrice);

        tripsQuery = tripsQuery.Where(t => t.Price >= startPrice && t.Price <= endPrice);

        if (!string.IsNullOrEmpty(q))
            tripsQuery = tripsQuery.Where(t => t.Title.Contains(q) || t.Description.Contains(q));

        if (startDate.HasValue)
            tripsQuery = tripsQuery.Where(t => t.StartDate.ToDateTime(TimeOnly.MinValue) >= startDate.Value.ToDateTime(TimeOnly.MinValue));
        if (endDate.HasValue)
            tripsQuery = tripsQuery.Where(t => t.StartDate.ToDateTime(TimeOnly.MinValue) <= endDate.Value.ToDateTime(TimeOnly.MinValue));

        // 2. Materialize the filtered trips
        var tripsList = tripsQuery.ToList();

        // Get the total count of trips in the database
        int totalCount = tripsList.Count;

        // 3. Only get TripPlace and Image records for the filtered trips
        var tripIds = tripsList.Select(t => t.Id).ToList();

        var allTripPlaces = _unitOfWork.TripPlace.Query().Where(tp => tripIds.Contains(tp.TripsId)).ToList();
        var allImages = _unitOfWork.image.Query().Where(i => tripIds.Contains(i.tripId)).ToList();
 
        // 4. Assign navigation properties
        foreach (var trip in tripsList)
        {
            trip.TripPlaces = allTripPlaces.Where(tp => tp.TripsId == trip.Id).ToList();
            trip.Image = allImages.Where(i => i.tripId == trip.Id).ToList();
        }

        // 5. Filter by destination (in-memory, since TripPlaces are now assigned)
        if (!string.IsNullOrEmpty(destination))
        {
            int destinationId = int.Parse(destination);
            tripsList = tripsList.Where(t => t.TripPlaces.Any(tp => tp.PlaceId == destinationId)).ToList();
        }

        // Get all TripCategory and Category records for the filtered trips
        var allTripCategories = _unitOfWork.tripCategory.Query().Where(tc => tripIds.Contains(tc.tripId)).ToList();
        var allCategories = _unitOfWork.category.Query().ToList();
        
        //assign locations
        var allLocations = _unitOfWork.Place.Query().ToList();
        // images
        // var allImages = _unitOfWork.image.Query().ToList();
        // 6. Paging and projection to DTO
        var pagedTrips = tripsList
            .Skip(start)
            .Take(len)
            .Select(t => new TripDto
            {
                Id = t.Id,
                AgenceId = t.VendorId,
                Title = t.Title,
                Price = t.Price,
                Locations = allLocations
                    .Where(l => t.TripPlaces.Any(tp => tp.PlaceId == l.Id))
                    .Select(l => l.Name )
                    .ToList()
                ,
                StartDate = t.StartDate,
                Description = t.Description,
                Rating = t.Rating,
                Images = t.Image?.Select(img => img.ImageUrl).ToList() ?? new List<string>(),
                AvailableSets = t.AvailableSets,
                Categories = allTripCategories
                    .Where(tc => tc.tripId == t.Id)
                    .Select(tc => allCategories.FirstOrDefault(c => c.Id == tc.categoryId)?.Name)
                    .Where(name => name != null)
                    .Select(name => name!) // Use null-forgiving operator to ensure non-null values
                    .ToList(),
            })
            .ToList();

        return (pagedTrips, totalCount);
    }

    public Task<IEnumerable<TripDto>> GetTripsByAgencyIdAsync(int id)
    {
        var trips = _unitOfWork.Trip.GetAllAsync().Result.Where(t => t.VendorId == id).ToList();
        return Task.FromResult(_tripMapper.MapToTripDtoList(trips));
    }

    public Task<bool> ApproveTripAsync(int id, bool isApproved)
    {
        var trip = _unitOfWork.Trip.GetByIdAsync(id).Result;
        if (trip == null)
        {
            return Task.FromResult(false);
        }

        trip.Status = isApproved ? 1 : 2; // 1 for approved, 2 for rejected
        _unitOfWork.Trip.Update(trip);
        _unitOfWork.CompleteAsync();
        return Task.FromResult(true);
    }
}
