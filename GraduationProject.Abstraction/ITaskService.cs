using GraduationProject.Shared.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface ITaskService
    {
        Task<IEnumerable<PredefinedTaskDTO>> GetAllPredefinedTasksAsync();
        Task<IEnumerable<TaskTitleAndStatusDTO>> GetTitleAndStatusAsync(int childId);
        Task<bool> UpdateTaskStatusAsync(int taskId);
    }
}
