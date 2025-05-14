using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.IServices;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetMessagesBySenderAndReceiver(int senderId, int receiverId)
        {
            var messages = await _messageService.GetMessageByResiverAndSenderIDs(senderId, receiverId);
            if (messages == null)
                return NotFound();
            return Ok(messages);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMessagesByUserId(int userId)
        {
            var messages = await _messageService.GetMessagesByUserIdAsync(userId);
            if (messages == null)
                return NotFound();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDto)
        {
            var success = await _messageService.SendMessageAsync(messageDto);
            if (!success)
                return BadRequest();
            return Ok();
        }
    }
}
