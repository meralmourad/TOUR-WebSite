using System;
using Backend.Data;
using Backend.DTOs.PlaceDTOs;
using Backend.IServices;
using Backend.Models;

namespace Backend.Services;

public class PlaceService : IPlaceService
{
    private readonly IUnitOfWork _unitOfWork;
    public PlaceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<GetPlaceDTO> CreatePlace(CreatePlaceDTO placeDTO)
    {
        var IsplaceExists = _unitOfWork.Place.PlaceExists(placeDTO.Name);
        if (IsplaceExists)
            throw new Exception("Place already exists");
        
        var place = new Place
        {
            Name = placeDTO.Name,
            Description = placeDTO.Description,
            Country = placeDTO.Country,
            ImagesURL = placeDTO.ImageURL
        };
        await _unitOfWork.Place.AddAsync(place);
        await _unitOfWork.CompleteAsync();
        
       var placeToReturn = new GetPlaceDTO
        {
            Id = place.Id,
            Name = place.Name,
            Description = place.Description,
            Country = place.Country,
            ImageURL = place.ImagesURL
        };
        return placeToReturn;   
    }

    public async Task<bool> DeletePlace(int id)
    {
        var place = await _unitOfWork.Place.GetByIdAsync(id);
        if (place == null)
            throw new Exception("Place not found");
        
        _unitOfWork.Place.Delete(place);
        return true;
    }

    public async Task<List<GetPlaceDTO>> GetAllPlaces()
    {
        var places = await _unitOfWork.Place.GetAllAsync();
        if (places == null)
            throw new Exception("No places found");
        
        var placesToReturn = new List<GetPlaceDTO>();
        foreach (var place in places)
        {
            var placeToReturn = new GetPlaceDTO
            {
                Id = place.Id,
                Name = place.Name,
                Description = place.Description,
                Country = place.Country,
                ImageURL = place.ImagesURL
            };
            placesToReturn.Add(placeToReturn);
        }
        return placesToReturn;
    }

    public async Task<GetPlaceDTO?> GetPlaceById(int id)
    {
        var place = await _unitOfWork.Place.GetByIdAsync(id);
        if (place == null)
            throw new Exception("Place not found");
        
        var placeToReturn = new GetPlaceDTO
        {
            Id = place.Id,
            Name = place.Name,
            Description = place.Description,
            Country = place.Country,
            ImageURL = place.ImagesURL
        };
        return placeToReturn;
    }

    public async Task<bool> UpdatePlace(int id, UpdatePlaceDTO placeDTO)
    {
        var place = await _unitOfWork.Place.GetByIdAsync(id);
        if (place == null)
            throw new Exception("Place not found");
        
        place.Name = placeDTO.Name?? place.Name;
        place.Description = placeDTO.Description?? place.Description;
        place.Country = placeDTO.Country?? place.Country;
        place.ImagesURL = placeDTO.ImageURL?? place.ImagesURL;
        
        _unitOfWork.Place.Update(place);
        return true;
    }
}
