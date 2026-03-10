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
                await _taskService.UpdateTaskStatusAsync(taskId);
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

            return Ok(new { TasksCount = count });
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
            return Ok(AlltasksCount);
        }


    }
}