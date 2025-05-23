using System.Security.Claims;
using Backend.DTOs.TripDTOs;
using Backend.IServices;
using Backend.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers{

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;
        private readonly notificationSocket _nsw;
    private readonly ILogger<TripController> _logger;

    public TripController(ITripService tripService, ILogger<TripController> logger, notificationSocket nsw)
    {
        _nsw = nsw;
        _tripService = tripService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Agency")]
    public async Task<IActionResult> CreateTrip(
        [FromForm] CreateTripDTO tripDto,
        [FromForm] List<IFormFile> images)
    {
        if (tripDto == null || images == null || !images.Any())
        {
            return BadRequest("Invalid input data. Trip details and images are required.");
        }

        var imageUrls = new List<string>();
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        int index = 0;
        foreach (var image in images)
        {
            // Generate a unique name for each image based on the current date and time
            var uniqueName = $"{DateTime.Now:yyyyMMdd_HHmmss_fff}_{Guid.NewGuid()}{index}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imageUrls.Add($"/images/{uniqueName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving image: {FileName}", image.FileName);
                return StatusCode(500, "An error occurred while uploading images.");
            }
        }

        tripDto.Images = imageUrls.ToArray();
        try
        {
            var result = await _tripService.CreateTripAsync(tripDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating trip.");
            return StatusCode(500, "An error occurred while saving the trip. Please try again.");
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
    // [HttpGet]
    // public async Task<IActionResult> GetAllTrips()
    // {
    //     try
    //     {
    //         var trips = await _tripService.GetAllTripsAsync();
    //         return Ok(trips);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, ex.Message);
    //     }
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, [FromBody] UpdateTripDTO tripDto)
    {
        try
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                var trip = await _tripService.GetTripByIdAsync(id);
                if (trip == null || trip.AgenceId != int.Parse(userIdClaim))
                {
                    return StatusCode(403, "You are not authorized to update this trip.");
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
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                var trip = await _tripService.GetTripByIdAsync(id);
                if (trip == null || trip.AgenceId != int.Parse(userIdClaim))
                {
                    return StatusCode(403, "You are not authorized to update this trip.");
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

    // [HttpGet("agency/{id}")]
    // [Authorize(Roles = "Admin,Agency")]
    // public async Task<IActionResult> GetTripsByAgencyId(int id)
    // {
    //     try
    //     {
    //         var trips = await _tripService.GetTripsByAgencyIdAsync(id);
    //         return Ok(trips);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, ex.Message);
    //     }
    // }
    // admin can approve or reject trips
    [HttpPut("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveTrip(int id, [FromBody] int isApproved)
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