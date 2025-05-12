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
    Task<IEnumerable<TripDto>>  GetTripsByAgencyIdAsync(int id);
    (IEnumerable<TripDto> Trips, int TotalCount) SearchTripsByQuery(
        string? q, int start, int len,
        string? destination, DateOnly? startDate,
        DateOnly? endDate, int startPrice, int endPrice,
        bool isApproved, bool isAdmin, int agencyId);
    Task<bool> ApproveTripAsync(int id, int isApproved);
}
