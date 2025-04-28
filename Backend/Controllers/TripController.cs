using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,Agency")]
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
            // Check if the user is authorized to update the trip (e.g., only Admin can update trips or the agency)
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                // agency can update their own trips
                var trip = await _tripService.GetTripByIdAsync(id);
                if (trip == null || trip.AgenceId != int.Parse(userIdClaim))
                {
                    return Forbid("You are not authorized to update this trip.");
                }
            }
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
            // Check if the user is authorized to delete the trip (e.g., only Admin can delete trips or the agency)
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                // agency can delete their own trips
                var trip = await _tripService.GetTripByIdAsync(id);
                if (trip == null || trip.AgenceId != int.Parse(userIdClaim))
                {
                    return Forbid("You are not authorized to delete this trip.");
                }
            }
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