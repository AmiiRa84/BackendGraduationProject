using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
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
    public class ChildServices : IChildServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ChildProgressDTO> GetChildProgressAsync(int childId)
        {
            var child = await _unitOfWork.GetRepository<Child, int>()
        .GetAllAsync(query => query
            .Where(c => c.Id == childId)
            .Include(c => c.Parent)
                .ThenInclude(p => p.User)     
            .Include(c => c.Specialist)
                .ThenInclude(s => s.User)
  );

            var result = child.FirstOrDefault(c => c.Id == childId);

            if (result is null)
                throw new ChildNotFoundException(childId);

            return new ChildProgressDTO
            {

                SpecialistName=result.Specialist?.User.FullName,
                SpecialistEmail=result.Specialist?.User.Email,
                ParentName = result.Parent?.User?.FullName,
                ParentPhone = result.Parent?.User?.PhoneNumber,
                ParentEmail = result.Parent?.User?.Email,
                ChildName = result.Name,
                Age = result.Age,
                Description = result.Description
            };
        }

        public async Task<IEnumerable<ChildDTO>> GetChildrenBySpecialistIdAsync(int SpecialistId)
        {

            var repo = _unitOfWork.GetRepository<Child, int>();

            var children = await repo.GetAllAsync(q =>
                q.Where(c => c.SpecialistId == SpecialistId)
            );
            if (!children.Any()) throw new SpecialistNotFoundException(SpecialistId);
            return _mapper.Map<IEnumerable<ChildDTO>>(children);
        }

        public async Task<IEnumerable<ChildNameDTO>> GetChildrenByParentIdAsync(int ParentId)
        {

            var repo = _unitOfWork.GetRepository<Child, int>();

            var children = await repo.GetAllAsync(q =>
                q.Where(c => c.ParentId == ParentId)
            );
            if (!children.Any()) throw new ParentNotFoundException(ParentId);
            return children.Select(c => new ChildNameDTO
            {
                Id = c.Id,
                Name = c.Name
            });

        }
    }
}

