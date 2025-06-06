using LibraryManagement.Models;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            message.SentAt = DateTime.UtcNow;
            await _messageService.SendMessageAsync(message);
            return Ok(message);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] string user1, [FromQuery] string user2)
        {
            var messages = await _messageService.GetMessagesBetween(user1, user2);
            return Ok(messages);
        }
    }
}
