using System.Security.Claims;
using Backend.DTOs.BookingDTOs;
using Backend.IServices;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
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
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    var bk = await _bookingService.GetAllBookings();
                    return Ok(bk);
                }
                    var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    if (role == "Agency")
                    {
                        var agencyId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                        var bks = await _bookingService.GetBookingsByAgencyId(int.Parse(agencyId));
                        return Ok(bks);
                    }
                    var touristId = int.TryParse(userId, out var id) ? id : 0;
                    Console.WriteLine($"Tourist ID: {touristId}");
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
             
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                
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

        // [HttpGet("trip/{tripId}")]
        // [Authorize(Roles = "Admin,Agency")]
        // public async Task<IActionResult> GetBookingsByTripId(int tripId)
        // {
        //     try
        //     {
        //         var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //         var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        //         if (role == "Agency")
        //         {
        //             var bks = await _bookingService.GetBookingsByAgencyId(int.Parse(userId));
        //             return Ok(bks);
        //         }
        //         var bookings = await _bookingService.GetBookingsByTripId(tripId);
        //         return Ok(bookings);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        //     }
        // }
        [HttpPost]
        [Authorize(Roles = "Admin,Agency,Tourist")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDTO)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var intUserIdClaim = int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
                bookingDTO.TouristId = (int)intUserIdClaim;
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

                var booking = await _bookingService.GetBookingById(id);
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                
                if (userId != booking.TouristId.ToString() && 
                    !User.IsInRole("Admin") && !User.IsInRole("Agent"))
                    return Forbid();
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDTO bookingDTO)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var booking = await _bookingService.GetBookingById(id);
                
                if (booking == null)
                    return NotFound();
                    
                if (userId != booking.TouristId.ToString() && !User.IsInRole("Admin") && !User.IsInRole("Agent"))
                    return Forbid();
                
                bookingDTO.TouristId = int.Parse(userId);
                bookingDTO.TripId = booking.TripId;
                var bking = await _bookingService.UpdateBooking(id, bookingDTO);
                if (bking == null)
                    return NotFound();
                return Ok(bking);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> ApproveBooking(int id,[FromBody]int Approved)
        {
                Console.WriteLine("\n\n\n\n\n\n in: \n\n\n\n\n\n");    

            try
            {
                var booking = await _bookingService.ApproveBooking(id, Approved);
                if (booking == null)
                    return NotFound();

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    
        [HttpPut("rate/{id}")]
        public async Task<IActionResult> RateBooking(int id, [FromBody] int rating)
        {
            try
            {
                var booking = await _bookingService.RateBooking(id, rating);
                if (booking == null)
                    return NotFound();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }   
    }
    }
}
