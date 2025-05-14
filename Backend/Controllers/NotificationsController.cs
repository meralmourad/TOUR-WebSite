using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly Notificationservices _notificationServices;

        public NotificationsController(Notificationservices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationServices.GetAllNotificationsAsync();
            if (notifications == null)
                return NotFound();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationServices.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpGet("receiver/{receiverId}")]
        public async Task<IActionResult> GetNotificationsByReceiverId(int receiverId, [FromQuery] int start = 0, [FromQuery] int len = 10)
        {
            var notifications = await _notificationServices.GetNotificationsByReceiverIdAsync(receiverId);

            if (notifications == null)
                return NotFound();
            var not = notifications.Skip(start).Take(len).ToList();
            return Ok(not);
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto notificationDto, [FromQuery] List<int> receiverIds)
        {
            var result = await _notificationServices.SendNotificationAsync(notificationDto, receiverIds,true);
            if (!result)
                return BadRequest();
            return Ok();
        }
        //service to send notification to trip tourist
        [HttpPost("trip/{tripId}")]
        public async Task<IActionResult> SendNotificationToTripTourist(int tripId, [FromBody] NotificationDto notificationDto)
        {
            if (notificationDto == null || string.IsNullOrEmpty(notificationDto.Context) || notificationDto.SenderId <= 0)
            {
                return BadRequest("Invalid notification data.");
            }

            if (tripId <= 0)
            {
                return BadRequest("Invalid trip ID.");
            }

            var result = await _notificationServices.SendNotificationToTripMembersAsync(notificationDto, tripId);
            if (!result)
            {
                return BadRequest("Failed to send notifications to trip members.");
            }

            return Ok("Notifications sent successfully.");
        }
    }
}
