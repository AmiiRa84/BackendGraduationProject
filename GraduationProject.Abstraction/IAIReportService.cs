using GraduationProject.Shared.DTOs.ReportDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using GraduationProject.Shared.DTOs.TaskGeneratedByAiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraduationProject.Services.Abstraction
{
    public interface IAIReportService
    {
        Task SaveTaskResultAsync(SaveTaskResultDto request);
        Task<FinalReportResponseDto> GenerateStructuredReportAsync(int childId);
        Task SaveGeneratedReportAsync(int childId, FinalReportResponseDto reportDto);
        Task SaveAvatarReportAsync(SaveAvatarReportDto request);
        Task<AiTaskResponseDto> GenerateTaskSuggestionAsync(string userPrompt);
        Task<IEnumerable<ChildReportDto>> GetChildReportsAsync(int childId);
    }
}
