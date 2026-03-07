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
    public class TaskController:ControllerBase
    {
      
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredefinedTaskDTO>>> GetAllPredefinedTasks()
        {
            var predefinedTasks =await _taskService.GetAllPredefinedTasksAsync();
            return Ok(predefinedTasks);
        }
    }
}
