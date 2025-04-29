using Backend.DTOs.PlaceDTOs;
using Backend.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }
        
        [HttpPost("create")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> CreatePlace([FromBody] CreatePlaceDTO placeDTO)
        {
            try
            {
                var place = await _placeService.CreatePlace(placeDTO);
                return CreatedAtAction(nameof(GetPlaceById), new { id = place.Id }, place);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaceById(int id)
        {
            try
            {
                var place = await _placeService.GetPlaceById(id);
                if (place == null)
                    return NotFound("Place not found");
                
                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlaces()
        {
            try
            {
                var places = await _placeService.GetAllPlaces();
                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            try
            {
                var result = await _placeService.DeletePlace(id);
                if (!result)
                    return NotFound("Place not found");
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
    }
}
