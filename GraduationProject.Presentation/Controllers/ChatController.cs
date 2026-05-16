using GraduationProject.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GraduationProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ISendbirdService _sendbirdService;
        private readonly ISendbirdSyncService _sendbirdSyncService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            ISendbirdService sendbirdService,
            ISendbirdSyncService sendbirdSyncService,
            ILogger<ChatController> logger)
        {
            _sendbirdService = sendbirdService;
            _sendbirdSyncService = sendbirdSyncService;
            _logger = logger;
        }

        // POST api/Chat/create-chat?parentId=13&specialistId=29
        [HttpPost("create-chat")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateChat(
            [FromQuery] int parentId,
            [FromQuery] int specialistId)
        {
            if (parentId <= 0 || specialistId <= 0)
                return BadRequest(new { success = false, message = "Valid parentId and specialistId are required." });
            try
            {
                var channelUrl = await _sendbirdService
                    .CreateChatBetweenParentAndSpecialistAsync(parentId, specialistId);

                return Ok(new
                {
                    success = true,
                    channelUrl = channelUrl
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "Failed to create chat between parent_{PId} & specialist_{SId}",
                    parentId, specialistId);
                return StatusCode(502, new { success = false, message = ex.Message });
            }
        }

        // POST api/Chat/sync-users
        [HttpPost("sync-users")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncAllUsers()
        {
            try
            {
                await _sendbirdSyncService.SyncAllUsersAsync();
                return Ok(new { success = true, message = "All users synced successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sendbird sync failed.");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}