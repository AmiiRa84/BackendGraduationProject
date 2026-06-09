using GraduationProject.Shared.DTOs.ChildDTOs;
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
        Task<bool> CompleteTaskWithNoteAsync(int taskId,string motherNote);
        Task<int> GetCountOfCompletedChildTask(int childId);
        Task<CreateManualTaskDTO> CreateManualTaskAsync(CreateManualTaskDTO dto);

        Task<int> GetAllTasksCountBySpecialistIdAsync(int specialistId);
        Task<IEnumerable<TasksDueTodayDTO>> GetTasksDueTodayAsync();

        Task<IEnumerable<ChildTaskDTO>> GetTasksByChildIdAsync(int childId);
        Task<List<TaskDetailsDTO>> GetTasksDetailsMobileApp(int childId);


    }
}
