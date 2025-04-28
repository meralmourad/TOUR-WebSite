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
        var users = _userService.SearchUsersByQuery(q, start, len, tourist, agency);
        return Ok(users);
    }

    [HttpGet("trips")]
    public IActionResult SearchTrips([FromQuery] int start, [FromQuery] int len, [FromQuery] string? destination, [FromQuery] DateTime? startDate, [FromQuery] string? q)
    {
        var trips = _tripService.SearchTripsByQuery(q, start, len, destination, startDate);
        return Ok(trips);
    }
}