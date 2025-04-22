using Backend.DTOs;
using Backend.DTOs.TripDTOs;

namespace Backend.IServices;

public interface ITripService
{
    Task<IEnumerable<TripDto>> GetAllTripsAsync();
    Task<TripDto?> GetTripByIdAsync(int id);
    Task<(bool Success, string Message)> CreateTripAsync(CreateTripDTO tripDto);
    Task<(bool Success, string Message)> UpdateTripAsync(int id, UpdateTripDTO tripDto);
    Task<(bool Success, string Message)> DeleteTripAsync(int id);
}
