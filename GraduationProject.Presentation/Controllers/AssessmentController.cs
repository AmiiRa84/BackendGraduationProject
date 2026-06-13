using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ReportDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly IAIReportService _aiReportService;

        public AssessmentController(IAIReportService aiReportService)
        {
            _aiReportService = aiReportService;
        }

        [HttpPost("generate-report/{childId}")]
        public async Task<IActionResult> GenerateReport(int childId)
        {
            var finalReport = await _aiReportService.GenerateStructuredReportAsync(childId);
            if (finalReport == null) return NotFound("Child not found.");

            await _aiReportService.SaveGeneratedReportAsync(childId, finalReport);

            return Ok(finalReport);
        }

        [HttpGet("child-reports/{childId}")]
        public async Task<IActionResult> GetChildReports(int childId)
        {
            var reports = await _aiReportService.GetChildReportsAsync(childId);
            if (!reports.Any()) return NotFound("No reports found for this child.");
            return Ok(reports);
        }

        [HttpPost("generate-ai-task")]
        public async Task<IActionResult> GenerateAiTask([FromQuery] string userPrompt)
        {
            if (string.IsNullOrWhiteSpace(userPrompt))
                return BadRequest("Please provide a prompt.");

            var result = await _aiReportService.GenerateTaskSuggestionAsync(userPrompt);
            return Ok(result);
        }

        [HttpPost("save-task-result")]
        public async Task<IActionResult> SaveResult([FromBody] SaveTaskResultDto request)
        {
            await _aiReportService.SaveTaskResultAsync(request);
            return Ok("Task result saved successfully.");
        }

        [HttpPost("save-avatar-report")]
        public async Task<IActionResult> SaveAvatarReport([FromBody] SaveAvatarReportDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.AvatarReport))
                return BadRequest("Avatar report is empty or invalid.");

            await _aiReportService.SaveAvatarReportAsync(request);
            return Ok("Avatar report saved successfully.");
        }
    }
}