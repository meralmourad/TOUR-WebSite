using Backend.DTOs;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers{

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO tripDto)
    {
        var result = await _tripService.CreateTripAsync(tripDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        var trip = await _tripService.GetTripByIdAsync(id);
        return trip != null ? Ok(trip) : NotFound("Trip not found.");
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        var trips = await _tripService.GetAllTripsAsync();
        return Ok(trips);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, [FromBody] UpdateTripDTO tripDto)
    {
        var result = await _tripService.UpdateTripAsync(id, tripDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var result = await _tripService.DeleteTripAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
}