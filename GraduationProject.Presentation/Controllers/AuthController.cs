using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs.AuthDTOs;
using GraduationProject.Shared.DTOs.AuthDTOs.Parent;
using GraduationProject.Shared.DTOs.AuthDTOs.Specialist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Success = false, msg = ex.Message });
            }
        }

        [HttpPost("register/parent")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterParent([FromBody] RegisterParentDTO dto)
        {
            var result = await _authService.RegisterParentAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/specialist")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterSpecialist([FromBody] RegisterSpecialistDTO dto)
        {
            var result = await _authService.RegisterSpecialistAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("specialist/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteSpecialist(int id)
        {
            try
            {
                await _authService.DeleteSpecialistAsync(id);
                return Ok(new { Success = true, msg = "Specialist deleted successfully" });
            }
            catch (SpecialistNotFoundException ex)
            {
                return NotFound(new { Success = false, msg = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, msg = ex.Message });
            }
        }

        [HttpPut("specialist/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSpecialist(int id, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                await _authService.UpdateSpecialistAsync(id, dto);
                return Ok(new { Success = true, msg = "Specialist updated successfully" });
            }
            catch (SpecialistNotFoundException ex)
            {
                return NotFound(new { Success = false, msg = ex.Message });
            }
        }

        [HttpPut("parent/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateParent(int id, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                var result = await _authService.UpdateParentAsync(id, dto);
                return Ok(result);
            }
            catch (ParentNotFoundException ex)
            {
                return NotFound(new { Success = false, msg = ex.Message });
            }
        }

        [HttpDelete("parent/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteParent(int id)
        {
            try
            {
                var result = await _authService.DeleteParentAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, msg = ex.Message });
            }
        }
        [HttpPost("verify-child-mode-password/{parentId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyChildModePassword(
            int parentId,
            [FromBody] VerifyChildModePasswordDTO dto)
        {
            try
            {
                var result = await _authService.VerifyChildModePasswordAsync(parentId, dto);

                if (!result.Success)
                    return Unauthorized(result);

                return Ok(result);
            }
            catch (ParentNotFoundExceptionAuth ex)
            {
                return NotFound(new
                {
                    Success = false,
                    msg = ex.Message
                });
            }
        }
    }
}