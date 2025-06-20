﻿using LibraryManagement.Dto.Request;
using LibraryManagement.Models;
using LibraryManagement.Service;
using LibraryManagement.Service.InterFace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
           [Authorize]
        public async Task<IActionResult> Send([FromBody] MessageRequest message)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderId == null) return BadRequest("Vui lòng đăng nhập");
            if (message == null) {
                return BadRequest("Vui lòng nhập tin nhắn");
            }
            Message messageSent = new Message
            {
                SenderId = senderId,
                ReceiverId = message.ReceiverId,
                SentAt = message.SentAt,
                Content = message.Content,
            };
            await _chatService.SendMessageAsync(messageSent);
            return Ok(messageSent);
        }

        [HttpGet("history")]
           [Authorize]
        public async Task<IActionResult> History([FromQuery] string receiveUserId) {
            var sendUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (sendUserId == null) return NotFound("Không tìm thấy thông tin người dùng");
            return Ok(await _chatService.GetAllMessagesAsync(sendUserId, receiveUserId));

        }
        [HttpGet("getAllUserSentMessage")]
        [Authorize]
        public async Task<IActionResult> GetAllUserMessage()
        {
            var sender = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (sender == null) return NotFound("Không tìm thấy thông tin người dùng");
            return Ok(await _chatService.getAllMessageClient(sender));
        }
    }
}