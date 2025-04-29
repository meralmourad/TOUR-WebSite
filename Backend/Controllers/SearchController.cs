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
    public IActionResult SearchUsers([FromQuery] int start, [FromQuery] int len, [FromQuery] bool? tourist, [FromQuery] bool? agency, [FromQuery] string? q)
    {
        // If both are null or both are false, search all
        if ((tourist == null && agency == null) || (tourist == false && agency == false))
        {
            tourist = true;
            agency = true;
        }
        if (tourist == null) tourist = false;
        if (agency == null) agency = false;

        if (len == 0 && start == 0)
        {
            len = int.MaxValue;
            start = 0;
        }
        var users = _userService.SearchUsersByQuery(q, start, len, tourist.Value, agency.Value);
        return Ok(users);
    }

    [HttpGet("trips")]
    public IActionResult SearchTrips(
    [FromQuery] int start = 0,
    [FromQuery] int len = 0,
    [FromQuery] string? destination = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] string? q = null)
    {
        // Set default pagination if not provided
        if (len == 0 && start == 0)
        {
            len = int.MaxValue;
            start = 0;
        }

        var trips = _tripService.SearchTripsByQuery(q, start, len, destination, startDate);
        return Ok(trips);
    }
}