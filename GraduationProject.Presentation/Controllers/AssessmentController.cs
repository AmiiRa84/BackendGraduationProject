using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.TaskDTOs;
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
    public class AssessmentController : ControllerBase
    {
        private readonly IAIReportService _aiReportService;
        private readonly IEmailService _emailService;

       
        public AssessmentController(IAIReportService aiReportService, IEmailService emailService)
        {
            _aiReportService = aiReportService;
            _emailService = emailService;
        }

        [HttpPost("generate-report/{childId}")]
        public async Task<IActionResult> GenerateReport(int childId)
        {
            
            var finalReport = await _aiReportService.GenerateStructuredReportAsync(childId);

            if (finalReport == null) return NotFound("الطفل غير موجود.");

            
            await _aiReportService.SaveReportAsync(childId, finalReport);

            return Ok(finalReport);
        }

        [HttpGet("child-reports/{childId}")]
        public async Task<IActionResult> GetChildReports(int childId)
        {
            // ✅ الـ Controller بينادي الـ Service بس
            var reports = await _aiReportService.GetChildReportsAsync(childId);

            if (reports == null || !reports.Any()) return NotFound("لا توجد تقارير لهذا الطفل.");

            return Ok(reports);
        }

        [HttpPost("generate-ai-task")]
        public async Task<IActionResult> GenerateAiTask([FromQuery] string userPrompt)
        {
            if (string.IsNullOrWhiteSpace(userPrompt))
                return BadRequest("يرجى كتابة ما تحتاجه في النشاط.");

            var result = await _aiReportService.GenerateTaskSuggestionAsync(userPrompt);

            return Ok(result);
        }

        [HttpPost("save-task-result")]
        public async Task<IActionResult> SaveResult([FromBody] SaveTaskResultDto request)
        {
            await _aiReportService.SaveTaskResultAsync(request);
            return Ok("تم حفظ نتيجة النشاط.");
        }
    }
}