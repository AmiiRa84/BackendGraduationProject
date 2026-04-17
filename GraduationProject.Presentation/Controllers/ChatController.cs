using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ChatDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ISendbirdService _sendbirdService;
        private readonly ISendbirdSyncService _syncService; // 👈 هنا

        public ChatController(ISendbirdService sendbirdService, ISendbirdSyncService syncService)
        {
            _sendbirdService = sendbirdService;
            _syncService = syncService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            var result = await _sendbirdService.CreateUserAsync(dto.UserId, dto.Nickname);
            return Ok(result);
        }

        [HttpPost("create-channel")]
        public async Task<IActionResult> CreateChannel([FromBody] CreateChannelDTO dto)
        {
            var result = await _sendbirdService.CreateChannelAsync(dto.ChannelName, dto.UserIds);
            return Ok(result);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO dto)
        {
            var result = await _sendbirdService.SendMessageAsync(dto.ChannelUrl, dto.Message, dto.UserId);
            return Ok(result);
        }

        [HttpPost("sync-users")]
        public async Task<IActionResult> SyncUsers()
        {
            await _syncService.SyncAllUsersAsync(); // 👈 هنا
            return Ok("Users synced to Sendbird successfully!");
        }

    }
}