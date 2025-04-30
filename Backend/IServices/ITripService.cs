using System;
using Backend.DTOs;
using Backend.DTOs.TripDTOs;
using Backend.Models;

namespace Backend.IServices;

public interface ITripService
{
    Task<(bool Success, string Message)> CreateTripAsync(CreateTripDTO tripDto);
    Task<(bool Success, string Message)> DeleteTripAsync(int id);
    Task<IEnumerable<TripDto>> GetAllTripsAsync();
    Task<TripDto?> GetTripByIdAsync(int id);
    Task<(bool Success, string Message)> UpdateTripAsync(int id, UpdateTripDTO tripDto);
    Task<IEnumerable<Trip>> SearchTripsAsync(int start, int len, string? destination, DateTime? startDate);
    IEnumerable<TripDto> SearchTripsByQuery(string? q, int start, int len, string? destination, DateTime? startDate, int startPrice, int endPrice);
}
