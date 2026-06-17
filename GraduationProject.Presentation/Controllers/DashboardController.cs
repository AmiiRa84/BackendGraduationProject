using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.DashboardDTO;
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
    public class DashboardController:ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("specialist/{specialistId}")]
        public async Task<ActionResult<List<ParentWithChildrenDTO>>> GetBySpecialist(int specialistId)
        {
            try
            {
               
                var results = await _dashboardService.GetParentsWithChildrenAndTasksBySpecialistIdAsync(specialistId);

                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "No parents found for this specialist." });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }
    }
}
    