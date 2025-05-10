using Backend.Data;
using Backend.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPlaceService _placeService;
    private readonly ITripService _tripService;
    private readonly IBookingService _bookingService;

    public SearchController(IUserService userService, IPlaceService placeService, ITripService tripService, IBookingService bookingService)
    {
        _userService = userService;
        _placeService = placeService;
        _tripService = tripService;
        _bookingService = bookingService;
    }

    [HttpGet("users")]
    public IActionResult SearchUsers(
        [FromQuery] int start,
        [FromQuery] int len,
        [FromQuery] bool? tourist,
        [FromQuery] bool? agency,
        [FromQuery] bool? admin,
        [FromQuery] string? q)
    {
        tourist ??= false;
        agency ??= false;
        admin ??= false;


        if (!User.Identity?.IsAuthenticated ?? true)
        {
            tourist = false;
            agency = true;
            admin = false;
        }

        if (User.IsInRole("Agency") )
        {
            tourist = false;
            agency = true;
            admin = false;
        }
        else if (User.IsInRole("Tourist"))
        {
            tourist = false;
            agency = true;
            admin = false;
        }

        if (len == 0 && start == 0)
        {
            len = int.MaxValue;
            start = 0;
        }

        var users = _userService.SearchUsersByQuery(q, start, len, tourist.Value, agency.Value, admin.Value);
        return Ok(users);
    }

    [HttpGet("trips")]
    public IActionResult SearchTrips(
    [FromQuery] int start = 0,
    [FromQuery] int len = int.MaxValue,
    [FromQuery] string? destination = null,
    [FromQuery] DateOnly? startDate = null,
    [FromQuery] DateOnly? endDate = null,
    [FromQuery] int Price = int.MaxValue,
    [FromQuery] bool? IsApproved=true,
    [FromQuery] string? q = null)
    {
        // check if the user is authenticated
        if (!User.Identity?.IsAuthenticated ?? true|| User.IsInRole("Tourist"))
            IsApproved = true;
        var isAdmin = User.IsInRole("Admin");
        //get id from the token
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var intUserIdClaim = int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;

        var trips = _tripService.SearchTripsByQuery
            (q, start, len,
            destination, startDate, endDate,
            0, Price,(bool) IsApproved, isAdmin,userId);
        return Ok(trips);
        
    }

    [HttpGet("bookings")]
    public IActionResult SearchBookings(
    [FromQuery] int start = 0,
    [FromQuery] int len = int.MaxValue,
    [FromQuery] bool? IsApproved = null,
    [FromQuery] int? agencyId = null)
    {
        //if the user is not authenticated, set IsApproved to true
        if(!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        if (!User.Identity?.IsAuthenticated ?? true || User.IsInRole("Tourist"))
            IsApproved ??= true;

        //if the user is an agency check if he requested his own bookings
        var isAdmin = User.IsInRole("Admin");
        if (User.IsInRole("Agency"))
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var intUserIdClaim = int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
            agencyId ??= intUserIdClaim;
        }
        var bookings = _bookingService.SearchBookingsByQuery(start, len, IsApproved ?? false, isAdmin, agencyId);
        var totalCount = bookings.Result.Count;

        return Ok(new { TotalCount = totalCount, Bookings = bookings.Result });
    }
}