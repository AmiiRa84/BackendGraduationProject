using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();

            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.Child.SpecialistId == specialistId));
            if (!tasks.Any())
                throw new SpecialistNotFoundException(specialistId);

            return tasks.Count();

        }
      
        public async Task<int> GetCountOfCompletedChildTask(int childId)
        {
            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();

            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.ChildId == childId && t.TaskStatus == TStatus.Completed));

            return tasks.Count();
        }

        public async Task<IEnumerable<ChildTaskDTO>> GetTasksByChildIdAsync(int childId)
        {

            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();

            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.ChildId == childId));
            if(!tasks.Any())
            {
                throw new ChildNotFoundException(childId);
            }
            return _mapper.Map<IEnumerable<ChildTaskDTO>>(tasks);

        }

        public async Task<IEnumerable<TasksDueTodayDTO>> GetTasksDueTodayAsync()
        {
            var today = DateTime.UtcNow.Date;

            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();

            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.DueDate.Date == today)
                 .Include(t => t.Child));

            return _mapper.Map<IEnumerable<TasksDueTodayDTO>>(tasks);

        }

        public async Task<IEnumerable<TaskTitleAndStatusDTO>> GetTitleAndStatusAsync(int childId)
        {
            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();

            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.ChildId == childId)
                 .Include(t => t.PreDefinedTask));
            if (!tasks.Any())
                throw new ChildNotFoundException(childId);

            return tasks.Select(task => new TaskTitleAndStatusDTO
            {
                Id = task.Id,
                Title = !string.IsNullOrEmpty(task.Title)
                    ? task.Title
                    : task.PreDefinedTask?.Title ?? "No Title",

                TaskStatus = task.TaskStatus.ToString()
            });
        }
        public async Task<bool> UpdateTaskStatusAsync(int taskId)
        {
            var task = await _unitOfWork
            .GetRepository<SpecialistTask, int>()
            .GetByIdAsync(taskId);

            if (task == null)
                throw new TaskNotFoundException(taskId);

            if (task.TaskStatus == TStatus.Completed)
                return false;

            task.TaskStatus = TStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<SpecialistTask, int>().Update(task);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }
        public async Task<List<TaskDetailsDTO>> GetTasksDetailsMobileApp(int childId)
        {
            
            var childRepo = _unitOfWork.GetRepository<Child, int>();
            var child = await childRepo.GetByIdAsync(childId);
            if (child is null) throw new ChildNotFoundException(childId);
         
            var repo = _unitOfWork.GetRepository<SpecialistTask, int>();
            var tasks = await repo.GetAllAsync(q =>
                q.Where(t => t.ChildId == childId)
                 .Include(t => t.PreDefinedTask));

            
            if (!tasks.Any())
                return new List<TaskDetailsDTO>
        {
            new TaskDetailsDTO
            {
                Message = "No assigned tasks yet."
            }
        };

         
            return tasks.Select(t => new TaskDetailsDTO
            {
                TaskId = t.Id,
                Title = !string.IsNullOrWhiteSpace(t.Title)
                    ? t.Title
                    : t.PreDefinedTask?.Title ?? "Untitled Task",
                Description = !string.IsNullOrWhiteSpace(t.Description)
                    ? t.Description
                    : t.PreDefinedTask?.Description ?? "No Description",
                AssignedDate = t.AssignedDate,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                PlannedDays = (t.DueDate - t.AssignedDate).TotalDays,
                ActualDays = t.CompletedAt.HasValue
                    ? (t.CompletedAt.Value - t.AssignedDate).TotalDays
                    : null,
                Status = t.TaskStatus == TStatus.Completed
                    ? "Completed"
                    : "Pending",
                PunctualityStatus = !t.CompletedAt.HasValue
                    ? (DateTime.UtcNow <= t.DueDate
                        ? "Pending On Time"
                        : "Overdue")
                    : (t.CompletedAt.Value <= t.DueDate
                        ? "On Time"
                        : "Late")
            }).ToList();
        }
    }
}
