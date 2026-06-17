using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

     
        [HttpPost("device-token")]
        public async Task<IActionResult> SaveDeviceToken([FromBody] DeviceTokenDTO dto)
        {
            try
            {
                await _notificationService.SaveDeviceTokenAsync(
                    dto.UserId,
                    dto.Role,
                    dto.Token,
                    dto.Platform);

                return Ok(new
                {
                    Success = true,
                    Message = "Token saved successfully"
                });
            }
            catch (ParentNotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (SpecialistNotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}