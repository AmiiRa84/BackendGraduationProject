using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GraduationProject.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateManualTaskDTO> CreateManualTaskAsync(CreateManualTaskDTO dto)
        {
          
            var child = await _unitOfWork.GetRepository<Child, int>()
                .GetByIdAsync(dto.ChildId)
                ?? throw new Exception("Child not found");

            if (child.SpecialistId != dto.SpecialistId)
                throw new Exception("This Specialist cannot assign task to this Child");

         
            SpecialistTask task;

            if (dto.PredefinedTaskId.HasValue)
            {
                var predefined = (await _unitOfWork.GetRepository<PreDefinedTask, int>()
                    .GetAllAsync(q => q.Where(p => p.Id == dto.PredefinedTaskId.Value)))
                    .FirstOrDefault()
                    ?? throw new Exception("Predefined task not found");

                task = new SpecialistTask
                {
                    Title = predefined.Title,
                    Description = predefined.Description,
                    TaskType = predefined.TaskType,
                    AssignedDate = DateTime.Now,
                    DueDate = dto.DueDate,
                    TaskStatus = TStatus.Pending,
                    Source = TaskSource.Predefined,
                    ChildId = child.Id,
                    PredefinedTaskId = predefined.Id
                    
                    
                };
            }
            else
            {
                if (!Enum.TryParse<TaskType>(dto.TaskType, true, out var taskTypeEnum))
                    throw new Exception("Invalid TaskType");

                task = new SpecialistTask
                {
                    Title = dto.Title!,
                    Description = dto.Description!,
                    TaskType = taskTypeEnum,
                    AssignedDate = DateTime.Now,
                    DueDate = dto.DueDate,
                    TaskStatus = TStatus.Pending,
                    Source = TaskSource.Custom,
                    ChildId = child.Id
                    
                    
                };
            }

         
            await _unitOfWork.GetRepository<SpecialistTask, int>().AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CreateManualTaskDTO>(task);
        }
            
        

        public async Task<IEnumerable<PredefinedTaskDTO>> GetAllPredefinedTasksAsync()
        {
            var predefinedTasks = await _unitOfWork.GetRepository<PreDefinedTask, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<PredefinedTaskDTO>>(predefinedTasks);
        }

        public async Task<int> GetAllTasksCountBySpecialistIdAsync(int specialistId)
        {
            var tasks = await _unitOfWork.GetRepository<SpecialistTask, int>()
        .GetAllAsync(query => query.Where(t => t.Child.SpecialistId == specialistId));

            return tasks.Count();

        }
      

        public async Task<int> GetCountOfCompletedChildTask(int childId)
        {
            var tasks = await _unitOfWork
                .GetRepository<SpecialistTask, int>()
                .GetAllAsync();

            return tasks.Count(t => t.ChildId == childId &&
                                    t.TaskStatus == TStatus.Completed);
        }

        public async Task<IEnumerable<ChildTaskDTO>> GetTasksByChildIdAsync(int childId)
        {
            
            var tasks =await _unitOfWork.GetRepository<SpecialistTask, int>().GetAllAsync();

          
            var childTasks = tasks.Where(t => t.ChildId == childId);

            return _mapper.Map<IEnumerable<ChildTaskDTO>>(childTasks);

        }

        public async Task<IEnumerable<TasksDueTodayDTO>> GetTasksDueTodayAsync()
        {
            var today = DateTime.Today;
            var taskDueToday =await _unitOfWork.GetRepository<SpecialistTask, int>()
                .GetAllAsync(include: q => q.Include(t => t.Child)
                .Where(t => t.DueDate == today));
         

           return _mapper.Map<IEnumerable<TasksDueTodayDTO>> (taskDueToday);
           
        }

        public async Task<IEnumerable<TaskTitleAndStatusDTO>> GetTitleAndStatusAsync(int childId)
        {
            var allTasks = await _unitOfWork.GetRepository<SpecialistTask, int>()
                .GetAllAsync(include: q => q.Include(t => t.PreDefinedTask));

            var result = allTasks.Select(task => new TaskTitleAndStatusDTO
            {
                Id = task.Id,
                Title = !string.IsNullOrEmpty(task.Title)
                            ? task.Title
                            : task.PreDefinedTask?.Title ?? "No Title",
                TaskStatus = task.TaskStatus.ToString()
            });

            return result;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId)
        {
            var task =await _unitOfWork.GetRepository<SpecialistTask, int>()
                .GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            if (task.TaskStatus == TStatus.Completed)
                throw new Exception("Task is already completed");

            task.TaskStatus = TStatus.Completed;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
