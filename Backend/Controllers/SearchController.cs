using System.Security.Claims;
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
        [FromQuery] bool? isApproved,
        [FromQuery] string? q)
    {
        tourist ??= false;
        agency ??= false;
        admin ??= false;
        isApproved ??= true;
        if(!(bool)isApproved && !User.IsInRole("Admin"))
        {
            isApproved = true;
        }

        if (!User.Identity?.IsAuthenticated ?? true)
        {
            tourist = false;
            agency = true;
            admin = false;
        }
        else if (User.IsInRole("Agency") )
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

        var result = _userService.SearchUsersByQuery(q, start, len, tourist.Value, agency.Value, admin.Value,(bool)isApproved);
return Ok(new { Users = result.Users, TotalCount = result.TotalCount });
    }

    [HttpGet("trips")]
    public IActionResult SearchTrips(
    [FromQuery] int start = 0,
    [FromQuery] int len = int.MaxValue,
    [FromQuery] string? destination = null,
    [FromQuery] DateOnly? startDate = null,
    [FromQuery] DateOnly? endDate = null,
    [FromQuery] int Price = int.MaxValue,
    [FromQuery] bool? IsApproved = true,
    [FromQuery] int? agencyId = 0,
    [FromQuery] string? q = null,
    [FromQuery] bool sortByRating = false)
    {
        if (endDate == null || (startDate.HasValue && endDate < startDate))
            endDate = DateOnly.MaxValue;
        //write user role
        Console.WriteLine("User role: " + User.IsInRole("Tourist") + " " + User.IsInRole("Agency") + " " + User.IsInRole("Admin"));
        //check if the user is authenticated
        if (!User.Identity?.IsAuthenticated ?? true)
            IsApproved = true;
        //if the user is a tourist, set IsApproved to true
        if (User.IsInRole("Tourist"))
            IsApproved = true;
        var isAdmin = User.IsInRole("Admin");
        // If the user is an agency, check if they requested their own trips
        if (User.IsInRole("Agency"))
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intUserIdClaim = int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
            agencyId = intUserIdClaim;
        }

        Console.WriteLine("\n\n\n\nisapproved: " + IsApproved+" agencyid "+ agencyId+"\n\n\n\n\n");
        var result = _tripService.SearchTripsByQuery(
            q, start, len,
            destination, startDate, endDate,
            0, Price, (bool)IsApproved, isAdmin, (int)agencyId);

        // If sortByRating is true, sort the trips by rating
        if (sortByRating)
        {
            result.Trips = result.Trips.OrderByDescending(t => t.Rating);
        }

        // Wrap the result in an object with named properties
        return Ok(new { Trips = result.Trips, TotalCount = result.TotalCount });
    }

    [HttpGet("bookings")]
    public IActionResult SearchBookings(
    [FromQuery] int start = 0,
    [FromQuery] int len = int.MaxValue,
    [FromQuery] bool? IsApproved = null,
    [FromQuery] int tripId = 0,
    [FromQuery] int? USERID = null)
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
            USERID ??= intUserIdClaim;
        }
        var bookings = _bookingService.SearchBookingsByQuery(start, len, IsApproved ?? false, isAdmin, USERID,tripId);
        var totalCount = bookings.Result.Count;

        return Ok(new { TotalCount = totalCount, Bookings = bookings.Result });
    }
}