using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.TaskDTOs;
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

        public TaskService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PredefinedTaskDTO>> GetAllPredefinedTasksAsync()
        {
            var predefinedTasks =await _unitOfWork.GetRepository<PreDefinedTask, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<PredefinedTaskDTO>>(predefinedTasks);
        }
    }
}
