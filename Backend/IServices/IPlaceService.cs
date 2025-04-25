using System;
using Backend.DTOs.PlaceDTOs;
namespace Backend.IServices;

public interface IPlaceService
{
    Task<GetPlaceDTO> CreatePlace(CreatePlaceDTO placeDTO);
    Task<bool> UpdatePlace(int id, UpdatePlaceDTO placeDTO);
    Task<bool> DeletePlace(int id);
    Task<GetPlaceDTO?> GetPlaceById(int id);
    Task<List<GetPlaceDTO>> GetAllPlaces();

}
