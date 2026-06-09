using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
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
    public class AIReportService:IAIReportService
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
                TotalMoves = request.TotalMoves,
                TimeTaken = request.TimeTaken,
                RoundsCount = request.RoundsCount,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.GetRepository<TaskResult, int>().AddAsync(result);

           
            var task = await _unitOfWork
                .GetRepository<SpecialistTask, int>()
                .GetByIdAsync(request.SpecialistTaskId);

            if (task != null)
            {
               
                task.TaskStatus = TStatus.Completed;
                task.CompletedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<SpecialistTask, int>().Update(task);
            }

            await _unitOfWork.SaveChangesAsync(); 
        }

        public async Task<FinalReportResponseDto> GenerateStructuredReportAsync(int childId)
        {
            var children = await _unitOfWork.GetRepository<Child, int>()
    .GetAllAsync(q => q.Where(c => c.Id == childId).Include(c => c.Tasks)); 
            var child = children.FirstOrDefault();

            if (child == null) return null!;

            var taskIds = child.Tasks.Select(t => t.Id).ToList();

  
            var childResults = await _unitOfWork.GetRepository<TaskResult, int>()
                .GetAllAsync(q => q.Where(r => taskIds.Contains(r.SpecialistTaskId))
                                   .OrderBy(r => r.CreatedAt));

            var aggregatedMotherNotes = new StringBuilder();
            var fullContext = new StringBuilder();

            foreach (var task in child.Tasks)
            {
                fullContext.AppendLine($"النشاط: {task.Title}. وصفه: {task.Description}.");

                
                if (!string.IsNullOrEmpty(task.MotherNote))
                {
                    aggregatedMotherNotes.Append($"{task.Title}: {task.MotherNote}. ");
                    fullContext.AppendLine($"ملاحظة الأم لنشاط {task.Title}: {task.MotherNote}");
                }

                var results = childResults.Where(r => r.SpecialistTaskId == task.Id).ToList();
                foreach (var res in results)
                {
                    fullContext.AppendLine($"أداء: {res.TimeTaken} ث، {res.TotalMoves} حركة.");
                }
            }

            var prompt = $@"أنت أخصائي مصري محترف. اكتب تقرير عن {child.Name}. البيانات: {fullContext}
[تعليمات صارمة]:
1. الرد JSON بـ 3 حقول: (summary, body, motherNote).
2. في حقل (motherNote): يجب كتابة الملاحظات بالتنسيق التالي: [اسم النشاط]: [الملاحظة]. اجمع كل الملاحظات ورا بعضها في سطر واحد.
3. ممنوع أي علامات Markdown أو رموز مثل * أو #.
4. ممنوع استخدام سطر جديد \n. اجعل الكلام جمل متصلة.
5. استبدل أي علامة تنصيص مزدوجة "" داخل الكلام بعلامة مفردة ' عشان الـ JSON ميبوظش.
 
[شكل الرد المطلوب]:
{{
  ""summary"": ""ملخص الحالة في سطر واحد متصل"",
  ""body"": ""التقرير والتوصيات في سطر واحد متصل"",
  ""motherNote"": ""[اسم النشاط]: [الملاحظة]. [اسم النشاط التالي]: [الملاحظة التالية].""
}}";

            var aiRawResult = await CallGeminiApi(prompt);

            try
            {
                var cleanJson = aiRawResult.Replace("```json", "").Replace("```", "").Trim();
                cleanJson = cleanJson.Replace("\n", " ").Replace("\r", " ");

                var aiData = JsonSerializer.Deserialize<Dictionary<string, string>>(cleanJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new FinalReportResponseDto
                {
                    ChildName = child.Name,
                    Summary = aiData?["summary"] ?? "تقرير مجمع",
                    Body = aiData?["body"] ?? "التفاصيل في التقرير",
                    MotherNote = string.IsNullOrEmpty(aiData?["motherNote"])
                        ? aggregatedMotherNotes.ToString().Trim()
                        : aiData["motherNote"]
                };
            }
            catch
            {
                return new FinalReportResponseDto
                {
                    ChildName = child.Name,
                    MotherNote = aggregatedMotherNotes.ToString().Trim(),
                    Summary = "تقرير متابعة شامل",
                    Body = aiRawResult.Replace("\n", " ").Replace("\"", "'")
                };
            }
        }

        private async Task<string> CallGeminiApi(string prompt)
        {
            var apiKey = (_config["GeminiSettings:ApiKey"] ?? "").Trim();
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var body = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                generationConfig = new
                {
                    temperature = 0.2,
                    topP = 0.8,
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return $"خطأ من جوجل: {response.StatusCode} - {errorDetails}";
            }

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "التقرير فارغ";
        }

        public async Task SaveReportAsync(int childId, FinalReportResponseDto reportDto)
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
                SpecialistName = r.Specialist?.User.FullName ?? "غير محدد"
            });
        }

        public async Task<AiTaskResponseDto> GenerateTaskSuggestionAsync(string userPrompt)
        {
            var systemPrompt = $@"أنت خبير تربوي متخصص في تصميم أنشطة ذكية ومختصرة للأطفال ذوي صعوبات التعلم.
المستخدم يريد نشاطاً لـ: {userPrompt}
 
[تعليمات التنسيق]
1. يجب أن يكون الرد بصيغة JSON حصراً.
2. لا تستخدم علامات الـ Markdown مثل ```json في الرد.
3. الـ title: يكون اسم جذاب ومختصر (لا يزيد عن 5 كلمات).
4. الـ description: يجب أن يكون منسقاً في نقاط (Bullet Points) ومختصراً جداً (لا يزيد عن 60 كلمة).
 
[هيكل الـ JSON المطلوب]
{{
  ""title"": ""اسم النشاط"",
  ""description"": ""• الخطوة الأولى.\n• الخطوة الثانية.\n• الهدف السلوكي من النشاط.""
}}";

            var jsonResult = await CallGeminiApi(systemPrompt);

            try
            {
                var cleanJson = jsonResult.Replace("```json", "").Replace("```", "").Trim();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<AiTaskResponseDto>(cleanJson, options)
                       ?? new AiTaskResponseDto { Title = "خطأ في التحويل", Description = "لم يتم استلام وصف" };
            }
            catch (JsonException)
            {
                return new AiTaskResponseDto
                {
                    Title = "نشاط مقترح",
                    Description = jsonResult
                };
            }
        }
    }
}

