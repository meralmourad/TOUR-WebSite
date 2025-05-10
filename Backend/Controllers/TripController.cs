using System.Security.Claims;
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
    public async Task<IActionResult> CreateTrip(
        [FromForm] CreateTripDTO tripDto
        ,[FromForm] List<IFormFile> images)
    {
    var imageUrls = new List<string>();
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

    foreach (var image in images)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }
        imageUrls.Add($"/images/{fileName}");
    }

    tripDto.Images = imageUrls.ToArray();
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

    //get all trips by agency id
    [HttpGet("agency/{id}")]
    [Authorize(Roles = "Admin,Agency")]
    public async Task<IActionResult> GetTripsByAgencyId(int id)
    {
        try
        {
            var trips = await _tripService.GetTripsByAgencyIdAsync(id);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    // admin can approve or reject trips
    [HttpPut("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveTrip(int id, [FromBody] bool isApproved)
    {
        try
        {
            var result = await _tripService.ApproveTripAsync(id, isApproved);
            return result ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

}