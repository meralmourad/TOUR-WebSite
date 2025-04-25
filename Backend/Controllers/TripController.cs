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
        try
        {
            var result = await _tripService.CreateTripAsync(tripDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        try
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            return trip != null ? Ok(trip) : NotFound("Trip not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        try
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, [FromBody] UpdateTripDTO tripDto)
    {
        try
        {
            var result = await _tripService.UpdateTripAsync(id, tripDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        try
        {
            var result = await _tripService.DeleteTripAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

}