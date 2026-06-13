using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Domain.Entities.ReportModule;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ReportDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using GraduationProject.Shared.DTOs.TaskGeneratedByAiDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class AIReportService : IAIReportService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AIReportService(HttpClient httpClient, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task SaveTaskResultAsync(SaveTaskResultDto request)
        {
         
            var result = new TaskResult
            {
                SpecialistTaskId = request.SpecialistTaskId,
                TotalMoves = request.TotalMoves ?? 0,
                TimeTaken = request.TimeTaken ?? 0,
                RoundsCount = request.RoundsCount ?? 0,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.GetRepository<TaskResult, int>().AddAsync(result);

       
            var task = await _unitOfWork.GetRepository<SpecialistTask, int>().GetByIdAsync(request.SpecialistTaskId);


            if (task != null)
            {
                task.MotherNote = request.MotherNote;
                task.TaskStatus = TStatus.Completed;
                _unitOfWork.GetRepository<SpecialistTask, int>().Update(task);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FinalReportResponseDto> GenerateStructuredReportAsync(int childId)
        {
            var children = await _unitOfWork.GetRepository<Child, int>()
                .GetAllAsync(q => q.Include(c => c.Tasks).Where(c => c.Id == childId));

            var child = children.FirstOrDefault();

            if (child == null) return null!;

       
            var taskIds = child.Tasks.Select(t => t.Id).ToList();
            var allResults = await _unitOfWork.GetRepository<TaskResult, int>().GetAllAsync();
            var childResults = allResults
                .Where(r => taskIds.Contains(r.SpecialistTaskId))
                .OrderBy(r => r.CreatedAt)
                .ToList();

            var allAvatarReports = await _unitOfWork.GetRepository<AvatarSessionReport, int>().GetAllAsync();
            var latestAvatarReport = allAvatarReports
                .Where(r => r.ChildId == childId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefault();

        
            bool hasCompletedTasks = child.Tasks.Any(t => t.TaskStatus == TStatus.Completed);
            bool hasAvatarReport = latestAvatarReport != null && !string.IsNullOrEmpty(latestAvatarReport.AvatarReport);

            if (!hasCompletedTasks && !hasAvatarReport)
            {
                return new FinalReportResponseDto
                {
                    ChildName = child.Name,
                    Summary = "N/A",
                    Body = "N/A",
                    MotherNote = "N/A"
                };
            }
     

            var aggregatedMotherNotes = new StringBuilder();
            var fullContext = new StringBuilder();

            foreach (var task in child.Tasks.Where(t => t.TaskStatus == TStatus.Completed))
            {
                fullContext.AppendLine($"Task Title: {task.Title}. Description: {task.Description}. Status: {task.TaskStatus}.");

                if (!string.IsNullOrEmpty(task.MotherNote))
                {
                    aggregatedMotherNotes.Append($"{task.Title}: {task.MotherNote}. ");
                    fullContext.AppendLine($"- Mother general note on this activity: {task.MotherNote}");
                }

                var results = childResults.Where(r => r.SpecialistTaskId == task.Id).ToList();
                foreach (var res in results)
                {
                    fullContext.AppendLine($"- Child performance: {res.TimeTaken}s, {res.TotalMoves} moves, rounds: {res.RoundsCount}.");
                }
            
        }

            if (hasAvatarReport)
            {
                fullContext.AppendLine($"---");
                fullContext.AppendLine($"General speech report from the AI Avatar system about the child's pronunciation and speech during the session: {latestAvatarReport!.AvatarReport}");
                fullContext.AppendLine($"---");
            }

           
            var prompt = $@"You are a highly professional speech and behavior specialist.
Your task is to write a comprehensive report about the child {child.Name}. The available data includes: a record of specific motor activities with their completion status, mother notes on them, and the general speech report from the AI Avatar system.

[Full Session Data and Context]:
{fullContext}

[Strict Formatting Instructions - CRITICAL]:
1. Language: Simple English (polite and professional).
2. The response must be a JSON Object only with three exclusive fields: (summary, body, motherNote).
3. (body) field: Write ONLY based on the data provided. Analyze tasks where Status is Completed and performance data exists. 
   CRITICAL: If a task has multiple performance records over time, COMPARE the sessions chronologically. 
   Clearly highlight the child's progress or regression (e.g., whether the time taken, number of moves, or rounds increased or decreased from the previous weeks). 
   Provide a clinical behavior and speech analysis based on this progression. If no tasks are completed, write exactly: N/A
4. DO NOT FABRICATE OR INVENT INFORMATION. If the [Full Session Data and Context] section is empty or does not contain tasks with 'Status: Completed', you MUST write exactly ""N/A"" for the 'body' and 'summary' fields.
5. If there are no notes explicitly written after 'Mother general note on this activity:', the 'motherNote' field MUST be exactly ""N/A"". Never invent a note like 'The child is improving' or 'Mother noted good behavior'.
6. Only extract information that is explicitly stated. Any assumption or extrapolation will be considered a severe system failure.
7. NEVER mark any activity as completed unless its Status in the data explicitly says Completed.
8. Markdown symbols are strictly forbidden (no asterisks * or hashtags #).
9. New lines (\n) or (\r) inside any field are strictly forbidden; keep all text as connected sentences.
10. Replace any double quotation marks "" inside the report text with a single quote ' to avoid breaking the JSON.

[Required Response Format]:
{{
  ""summary"": ""session summary here in one continuous line, or N/A"",
  ""body"": ""full report here in one continuous line, or N/A"",
  ""motherNote"": ""[activity name]: [note]. [next activity]: [next note]. or N/A""
}}";

            var aiRawResult = await CallOpenAiApi(prompt);
            Dictionary<string, string>? aiData = null;

            try
            {
                var cleanJson = aiRawResult.Replace("```json", "").Replace("```", "").Trim();
                cleanJson = cleanJson.Replace("\n", " ").Replace("\r", " ");

                aiData = JsonSerializer.Deserialize<Dictionary<string, string>>(cleanJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                
                aiData = new Dictionary<string, string>
                {
                    { "summary", "N/A" },
                    { "body", "N/A" },
                    { "motherNote", "N/A" }
                };
            }

            var motherNoteValue = aggregatedMotherNotes.Length > 0
                ? aggregatedMotherNotes.ToString().Trim()
                : "No mother notes available.";

            return new FinalReportResponseDto
            {
                ChildName = child!.Name,
                Summary = aiData?["summary"] ?? "N/A",
                Body = aiData?["body"] ?? "N/A",
                MotherNote = (string.IsNullOrEmpty(aiData?["motherNote"]) || aiData?["motherNote"] == "N/A")
                    ? motherNoteValue
                    : aiData["motherNote"]
            };
        }

        public async Task SaveGeneratedReportAsync(int childId, FinalReportResponseDto reportDto)
        {
            var child = await _unitOfWork.GetRepository<Child, int>().GetByIdAsync(childId);
            if (child == null) return;

            var reportEntity = new Report
            {
                Content = $"{reportDto.Summary} \n {reportDto.Body}",
                ChildId = childId,
                AssignedDate = DateTime.Now,
                SpecialistId = child.SpecialistId,
                ParentId = child.ParentId
            };

            await _unitOfWork.GetRepository<Report, int>().AddAsync(reportEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SaveAvatarReportAsync(SaveAvatarReportDto request)
        {
            var avatarReport = new AvatarSessionReport
            {
                ChildId = request.ChildId,
                AvatarReport = request.AvatarReport,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.GetRepository<AvatarSessionReport, int>().AddAsync(avatarReport);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChildReportDto>> GetChildReportsAsync(int childId)
        {
            var reports = await _unitOfWork.GetRepository<Report, int>()
                .GetAllAsync(q => q.Where(r => r.ChildId == childId)
                                   .OrderByDescending(r => r.AssignedDate)
                                   .Include(r => r.Specialist)
                                       .ThenInclude(s => s.User));

            return reports.Select(r => new ChildReportDto
            {
                Id = r.Id,
                Content = r.Content,
                AssignedDate = r.AssignedDate,
                SpecialistName = r.Specialist?.User.FullName ?? "Not specified"
            });
        }

        public async Task<AiTaskResponseDto> GenerateTaskSuggestionAsync(string userPrompt)
        {
            var systemPrompt = $@"You are an educational expert specialized in designing smart and concise activities for children with learning difficulties.
The user wants an activity for: {userPrompt}

[Formatting Instructions]
1. The response must be in JSON format only.
2. Do not use Markdown tags like ```json in the response.
3. title: a catchy and short name (no more than 5 words).
4. description: must be formatted in bullet points and very brief (no more than 60 words).

[Required JSON Structure]
{{
  ""title"": ""Activity Name"",
  ""description"": ""• Step one.\n• Step two.\n• Behavioral goal of the activity.""
}}";

            var jsonResult = await CallOpenAiApi(systemPrompt);

            try
            {
                var cleanJson = jsonResult.Replace("```json", "").Replace("```", "").Trim();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<AiTaskResponseDto>(cleanJson, options)
                       ?? new AiTaskResponseDto { Title = "Conversion error", Description = "No description received" };
            }
            catch (JsonException)
            {
                return new AiTaskResponseDto
                {
                    Title = "Suggested Activity",
                    Description = jsonResult
                };
            }
        }

   
        private async Task<string> CallOpenAiApi(string prompt)
        {
            var apiKey = (_config["OpenAISettings:ApiKey"] ?? "").Trim();

           
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new { role = "user", content = prompt }
        },
                temperature = 0.2,
                max_tokens = 1500
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

       
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return $"OpenAI Error: {response.StatusCode} - {errorDetails}";
            }

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);

            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "Empty report";
        }
    }
}