using System;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.PlaceDTOs;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Backend.Mappers;
using Backend.Models;
using Backend.WebSockets;

namespace Backend.Services;

public class TripService : ITripService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TripMapper _tripMapper;
    private readonly Notificationservices _notificationServices;
    private readonly notificationSocket _nws; 
    private readonly MyWebSocketManager _ws; 
    private static int imageIndex = 1;

    public TripService(IUnitOfWork unitOfWork, MyWebSocketManager ws, notificationSocket notificationSocket,
        Notificationservices notificationServices)
    {
        _notificationServices = notificationServices;
        _unitOfWork = unitOfWork;
        _nws = notificationSocket;
        _ws = ws;
        _tripMapper = new TripMapper(unitOfWork);
    }

    private void ThrowIfErrorFound(bool condition, string errorMessage)
    {
        if (condition)
            throw new Exception(errorMessage);
    }

    public async Task<(bool Success, string Message)> CreateTripAsync(CreateTripDTO tripDto)
    {
        try
        {
            ValidateCreateTripDto(tripDto);

            if (_notificationServices == null)
            {
                Console.WriteLine("Error: Notification services (_notificationServices) is null."); 
                return (false, "Notification service is unavailable.");
            }

            if (tripDto.LocationIds == null || !tripDto.LocationIds.Any())
            {
                Console.WriteLine("Error: Location IDs are null or empty.");
                return (false, "At least one location is required.");
            }

            if (tripDto.CategoryIds == null || !tripDto.CategoryIds.Any())
            {
                Console.WriteLine("Error: Category IDs are null or empty.");
                return (false, "At least one category is required.");
            }

            if (tripDto.Images == null || !tripDto.Images.Any())
            {
                Console.WriteLine("Error: Images are null or empty.");
                return (false, "At least one image is required.");
            }

            var trip = new Trip
            {
                VendorId = tripDto.AgenceId,
                Title = tripDto.Title,
                Price = tripDto.Price,
                StartDate = tripDto.StartDate,
                EndDate = tripDto.EndDate,
                Description = tripDto.Description ?? string.Empty,
                Rating = 0,
                Status = 0,
                AvailableSets = tripDto.AvailableSets,
            };
            await _unitOfWork.Trip.AddAsync(trip);

            await _unitOfWork.CompleteAsync();
            Console.WriteLine("-----------------------------------Trip created with ID: " + trip.Id); 

            await AddTripLocations(trip.Id, tripDto.LocationIds);

            
            await AddTripCategories(trip.Id, tripDto.CategoryIds);

            await AddTripImages(trip.Id, tripDto.Images);

            await _unitOfWork.CompleteAsync();

            var agency = await _unitOfWork.User.GetByIdAsync(tripDto.AgenceId);
            if (agency == null)
            {
                Console.WriteLine("Error: Agency not found.");
                return (false, "Agency not found.");
            }
            var agencyName = agency.Name;
            if (string.IsNullOrEmpty(agencyName))
            {
                Console.WriteLine("Error: Agency name is null or empty.");
                return (false, "Agency name is invalid.");
            }
            Console.WriteLine("Agency Name: " + agencyName); 

            if (_nws == null)
            {
                Console.WriteLine("Error: Notification socket (_nws) is null.");
                return (false, "Notification service is unavailable.");
            }

            var nofi = new {
                content = agencyName + " has created Trip.",
                TripId = trip.Id,
            };
            var nofiJson = System.Text.Json.JsonSerializer.Serialize(nofi);
            Console.WriteLine("Notification JSON: " + nofiJson); 
            await _nws.SendMessageToUserAsync(1, nofiJson);

            var dbnotif = new NotificationDto
            {
                ReceiverId = 1,
                SenderId = tripDto.AgenceId,
                Context = agencyName + " has created a Trip.",
            };
            await _notificationServices.SendNotificationAsync(dbnotif, new List<int> { 1 },false);
            await _unitOfWork.CompleteAsync();

            return (true, "Trip created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in CreateTripAsync: " + ex.Message); 
            return (false, "An error occurred while saving the trip. Please try again.");
        }
    }

    private void ValidateCreateTripDto(CreateTripDTO tripDto)
    {
        ThrowIfErrorFound(string.IsNullOrWhiteSpace(tripDto.Title), "Title is required.");
        ThrowIfErrorFound(tripDto.Price <= 0, "Price must be greater than zero.");
        ThrowIfErrorFound(tripDto.StartDate == default, "Start date is required.");
        ThrowIfErrorFound(tripDto.LocationIds == null || !tripDto.LocationIds.Any(), "At least one location is required.");
    }

    private async Task AddTripLocations(int tripId, IEnumerable<int> locationIds)
    {
        var tripPlaces = locationIds.Select(location => new TripPlace
        {
            TripsId = tripId,
            PlaceId = location
        }).ToList();

        foreach (var tripPlace in tripPlaces)
        {
            await _unitOfWork.TripPlace.AddAsync(tripPlace);
        }
    }

    private async Task AddTripCategories(int tripId, IEnumerable<int> categoryIds)
    {
        var tripCategories = categoryIds.Select(category => new TripCategory
        {
            tripId = tripId,
            categoryId = category
        }).ToList();

        foreach (var tripCategory in tripCategories)
        {
           await _unitOfWork.tripCategory.AddAsync(tripCategory);
        }
    }

    private async Task AddTripImages(int tripId, IEnumerable<string> images)
    {
        foreach (var imageUrl in images)
        {
            var image = new Images
            {
                ImageUrl = imageUrl,
                tripId = tripId
            };
            await _unitOfWork.image.AddAsync(image);
        }
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
        var curTrip = await _unitOfWork.Trip.GetByIdAsync(id);
        ThrowIfErrorFound(curTrip == null, "Trip not found.");

        curTrip.Title = tripDto.Title ?? curTrip.Title;
        curTrip.Price = tripDto.Price ?? curTrip.Price;
        curTrip.StartDate = tripDto.StartDate ?? curTrip.StartDate;
        curTrip.EndDate = tripDto.EndDate ?? curTrip.EndDate;
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
        var tripsQuery = _unitOfWork.Trip.Query();
        int Approved = isApproved ? 1 : 0;
            if (isAdmin)
                tripsQuery = tripsQuery.Where(t => t.Status == Approved);
            else
                if(agencyId!=0)
                    tripsQuery = tripsQuery.Where(t => t.Status == Approved && t.VendorId == agencyId);
                else 
                    tripsQuery = tripsQuery.Where(t => t.Status == Approved);
            
        if(agencyId != 0)
        {
            tripsQuery = tripsQuery.Where(t => t.VendorId == agencyId);
        }
        start = Math.Max(0, start);
        len = Math.Max(1, len);
        startPrice = Math.Max(0, startPrice);
        endPrice = Math.Max(0, endPrice);

        tripsQuery = tripsQuery.Where(t => t.Price >= startPrice && t.Price <= endPrice);

        if (!string.IsNullOrEmpty(q))
            tripsQuery = tripsQuery.Where(t => t.Title.Contains(q) || t.Description.Contains(q));

        if (startDate.HasValue || endDate.HasValue)
        {
            var tripsList = tripsQuery.ToList();

            if (startDate.HasValue)
                tripsList = tripsList.Where(t => t.StartDate >= startDate.Value).ToList();
            if (endDate.HasValue)
                tripsList = tripsList.Where(t => t.StartDate <= endDate.Value).ToList();



            var tripIds = tripsList.Select(t => t.Id).ToList();

            var allTripPlaces = _unitOfWork.TripPlace.Query().Where(tp => tripIds.Contains(tp.TripsId)).ToList();
            var allImages = _unitOfWork.image.Query().Where(i => tripIds.Contains(i.tripId)).ToList();
 
            foreach (var trip in tripsList)
            {
                trip.TripPlaces = allTripPlaces.Where(tp => tp.TripsId == trip.Id).ToList();
                trip.Image = allImages.Where(i => i.tripId == trip.Id).ToList();
            }

            if (!string.IsNullOrEmpty(destination))
            {
                int destinationId = int.Parse(destination);
                tripsList = tripsList.Where(t => t.TripPlaces.Any(tp => tp.PlaceId == destinationId)).ToList();
            }

            var allTripCategories = _unitOfWork.tripCategory.Query().Where(tc => tripIds.Contains(tc.tripId)).ToList();
            var allCategories = _unitOfWork.category.Query().ToList();
            var allLocations = _unitOfWork.Place.Query().ToList();
            int totalCount = tripsList.Count;
            
            var pagedTrips = tripsList
                .OrderByDescending(t => t.Rating)
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
                    Status = t.Status,
                    StartDate = t.StartDate,
                    Description = t.Description,
                    Rating = t.Rating,
                    Images = t.Image?.Select(img => img.ImageUrl).ToList() ?? new List<string>(),
                    AvailableSets = t.AvailableSets,
                    Categories = allTripCategories
                        .Where(tc => tc.tripId == t.Id)
                        .Select(tc => allCategories.FirstOrDefault(c => c.Id == tc.categoryId)?.Name)
                        .Where(name => name != null)
                        .Select(name => name!)
                        .ToList(),
                })
                .ToList();

            return (pagedTrips, totalCount);
        }
        else
        {
            var tripsList = tripsQuery.ToList();
            int totalCount = tripsList.Count;

            var tripIds = tripsList.Select(t => t.Id).ToList();

            var allTripPlaces = _unitOfWork.TripPlace.Query().Where(tp => tripIds.Contains(tp.TripsId)).ToList();
            var allImages = _unitOfWork.image.Query().Where(i => tripIds.Contains(i.tripId)).ToList();
 
            foreach (var trip in tripsList)
            {
                trip.TripPlaces = allTripPlaces.Where(tp => tp.TripsId == trip.Id).ToList();
                trip.Image = allImages.Where(i => i.tripId == trip.Id).ToList();
            }

            if (!string.IsNullOrEmpty(destination))
            {
                int destinationId = int.Parse(destination);
                tripsList = tripsList.Where(t => t.TripPlaces.Any(tp => tp.PlaceId == destinationId)).ToList();
            }

            var allTripCategories = _unitOfWork.tripCategory.Query().Where(tc => tripIds.Contains(tc.tripId)).ToList();
            var allCategories = _unitOfWork.category.Query().ToList();
            var allLocations = _unitOfWork.Place.Query().ToList();

            var pagedTrips = tripsList
                .OrderByDescending(t => t.Rating)
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
                        .Select(name => name!)
                        .ToList(),
                })
                .ToList();

            return (pagedTrips, totalCount);
        }
    }

    public Task<IEnumerable<TripDto>> GetTripsByAgencyIdAsync(int id)
    {
        var trips = _unitOfWork.Trip.GetAllAsync().Result.Where(t => t.VendorId == id).ToList();
        return Task.FromResult(_tripMapper.MapToTripDtoList(trips));
    }

    public async Task<bool> ApproveTripAsync(int id, int isApproved)
    {
        try {
            await _ws.SendMessageToUserAsync(1, "i'm here ");
            var trip = await _unitOfWork.Trip.GetByIdAsync(id);
            if (trip == null)
                return false;
                
            trip.Status = isApproved;
            _unitOfWork.Trip.Update(trip);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception) {
            return false;
        }
    }
}
