using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ChildDTOs;
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
    [Route("api/[Controller]")]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet("predefined")]
        public async Task<ActionResult<IEnumerable<PredefinedTaskDTO>>> GetAllPredefinedTasks()
        {
            var predefinedTasks = await _taskService.GetAllPredefinedTasksAsync();
            if (predefinedTasks is null) return NotFound("Predefined Tasks is Not Found");
            
            return Ok(predefinedTasks);
        }
        [HttpGet("title-status/{childId}")]
        public async Task<ActionResult<IEnumerable<TaskTitleAndStatusDTO>>> GetTaskTitleAndStatus(int childId)
        {
            var TaskTitleAndStatus = await _taskService.GetTitleAndStatusAsync(childId);

            return Ok(TaskTitleAndStatus);
        }

        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId)
        {

            try
            {
                var result = await _taskService.UpdateTaskStatusAsync(taskId);

                if (!result)
                    return BadRequest("You've already completed this task");

                return Ok("Task status updated to Completed");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Task not found")
                    return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("child/{childId}/tasks-count")]
        public async Task<IActionResult> GetChildTasksCount(int childId)
        {
            var count = await _taskService.GetCountOfCompletedChildTask(childId);

            return Ok(new { CompletedTasksCount = count });
        }
        
        [HttpPost("manual-task")]
        public async Task<IActionResult> CreateManualTask([FromBody] CreateManualTaskDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskDto = await _taskService.CreateManualTaskAsync(dto);

            return Ok(taskDto);
        }

        [HttpGet("specialists/{specialistId}/tasks/count")]
        public async Task<IActionResult> GetAllTasksCountBySpecialistId(int specialistId)
        {
            var AlltasksCount =await _taskService.GetAllTasksCountBySpecialistIdAsync(specialistId);
            return Ok(new {TasksCountOfSpecialist=AlltasksCount });
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTasksDueToday()
        {
            var tasks = await _taskService.GetTasksDueTodayAsync();
            return Ok(tasks);
        }


        [HttpGet("child/{childId}")]
        public async Task<ActionResult<IEnumerable<ChildTaskDTO>>> GetTasksByChild(int childId)
        {
            var tasks = await _taskService.GetTasksByChildIdAsync(childId);

            if (tasks == null || !tasks.Any())
                return NotFound("No tasks found for this child.");

            return Ok(tasks);
        }

        [HttpGet("child/{childId}/details")]
        public async Task<IActionResult> GetTasksDetailsMobileApp(int childId)
        {
            var result = await _taskService.GetTasksDetailsMobileApp(childId);

            if (result == null || !result.Any())
                return NotFound("No tasks found for this child");

            return Ok(result);
        }

        }
}