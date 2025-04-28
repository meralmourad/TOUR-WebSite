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

}