using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.TripDTOs;
using Backend.Models;

namespace Backend.Mappers;

public class TripMapper
{
    private readonly IUnitOfWork _unitOfWork;

    public TripMapper(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public TripDto MapToTripDto(Trip trip)
    {
        var allTripPlaces = _unitOfWork.TripPlace.Query().Where(tp => tp.TripsId == trip.Id).ToList();
        var allImages = _unitOfWork.image.Query().Where(i => i.tripId == trip.Id).ToList();
        var allTripCategories = _unitOfWork.tripCategory.Query().Where(tc => tc.tripId == trip.Id).ToList();
        var allCategories = _unitOfWork.category.Query().ToList();
        var allLocations = _unitOfWork.Place.Query().ToList();

        return new TripDto
        {
            Id = trip.Id,
            AgenceId = trip.VendorId,
            Title = trip.Title,
            Price = trip.Price,
            Locations = allLocations
                .Where(l => allTripPlaces.Any(tp => tp.PlaceId == l.Id))
                .Select(l => l.Name)
                .ToList(),
            StartDate = trip.StartDate,
            Description = trip.Description,
            Rating = trip.Rating,
            Images = allImages.Select(img => img.ImageUrl).ToList(),
            AvailableSets = trip.AvailableSets,
            Categories = allTripCategories
                .Select(tc => allCategories.FirstOrDefault(c => c.Id == tc.categoryId)?.Name)
                .Where(name => name != null)
                .Select(name => name!)
                .ToList(),
        };
    }

    public IEnumerable<TripDto> MapToTripDtoList(IEnumerable<Trip> trips)
    {
        return trips.Select(MapToTripDto).ToList();
    }
}
