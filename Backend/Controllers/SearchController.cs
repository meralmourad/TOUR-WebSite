using Backend.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPlaceService _placeService;
    private readonly ITripService _tripService;

    public SearchController(IUserService userService, IPlaceService placeService, ITripService tripService)
    {
        _userService = userService;
        _placeService = placeService;
        _tripService = tripService;
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
    [FromQuery] DateTime? startDate = null,
    [FromQuery] int startPrice = 0,
    [FromQuery] int endPrice = int.MaxValue,
    [FromQuery] string? q = null)
    {
        var trips = _tripService.SearchTripsByQuery(q, start, len, destination, startDate, startPrice, endPrice);
        return Ok(trips);
        
    }
}