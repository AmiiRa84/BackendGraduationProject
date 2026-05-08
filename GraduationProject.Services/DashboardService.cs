using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.DashboardDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ParentWithChildrenDTO>> GetParentsWithChildrenAndTasksBySpecialistIdAsync(int specialistId)
        {

            var specialist = await _unitOfWork.GetRepository<Specialist, int>()
                .GetByIdAsync(specialistId);

            if (specialist is null)
                throw new SpecialistNotFoundException(specialistId);


            var parentsFromDb = await _unitOfWork.GetRepository<Parent, int>()
                .GetAllAsync(q => q
                    .Include(p => p.User)
                    .Include(p => p.Children)
                        .ThenInclude(c => c.Tasks));


            var result = parentsFromDb
                .Where(p => p.Children.Any(c => c.SpecialistId == specialistId))
                .Select(p => new ParentWithChildrenDTO
                {
                    Id = p.Id,
                    Name = p.User.FullName,
                    Email = p.User.Email!,
                    Children = p.Children
                        .Where(c => c.SpecialistId == specialistId)
                        .Select(c => new childDTO02
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Age = c.Age,
                            Description = c.Description,
                            Tasks =c.Tasks.Select(t => new TaskDTO02
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Description = t.Description,
                                AssignedDate = t.AssignedDate,
                                DueDate = t.DueDate,
                                TaskStatus = t.TaskStatus.ToString(),
                                TaskType = t.TaskType.ToString(),
                                Source = t.Source.ToString()
                            }).ToList()
                        }).ToList()
                }).ToList();


            return result;
        }


    }
}