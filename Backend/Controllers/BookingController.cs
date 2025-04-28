using Backend.DTOs.BookingDTOs;
using Backend.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                //if user is an admin return all bookings
                if (User.IsInRole("Admin"))
                {
                    var bk = await _bookingService.GetAllBookings();
                    return Ok(bk);
                }
                    var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                    var touristId = int.TryParse(userId, out var id) ? id : 0;
                    var bookings = await _bookingService.GetBookingsByTouristId(touristId);
                    return Ok(bookings);
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingById(id);
             
                var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                
                if (userId != booking.TouristId.ToString() && !User.IsInRole("Admin") && !User.IsInRole("Agent"))
                    return Forbid();

                if (booking == null)return NotFound();
                
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDTO)
        {
            try
            {
                var booking = await _bookingService.CreateBooking(bookingDTO);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                // check if the agent is the one who created the booking and is not an admin
                if (!User.IsInRole("Admin"))
                {
                    var booking = await _bookingService.GetBookingById(id);
                    var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                    if (userId != booking.TouristId.ToString() && !User.IsInRole("Admin") && !User.IsInRole("Agent"))
                        return Forbid();
                }
                var result = await _bookingService.DeleteBooking(id);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
