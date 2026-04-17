using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
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
                  .GetByIdAsync(childId);

            if (child == null) return null;

            return new ChildProgressDTO
            {
                Name = child.Name,
                Age = child.Age,
             
                Description = child.Description,
               
            };
        }

        public async Task<IEnumerable<ChildDTO>> GetChildrenBySpecialistIdAsync(int SpecialistId)
        {
           
            var allChildren = await _unitOfWork.GetRepository<Child, int>().GetAllAsync();

           
            var SpecialistChildren = allChildren.Where(c => c.SpecialistId == SpecialistId);

            return _mapper.Map<IEnumerable<ChildDTO>>(SpecialistChildren);
        }

        public async Task<IEnumerable<ChildNameDTO>> GetChildrenByParentIdAsync(int ParentId)
        {
      
            var allChildren = await _unitOfWork.GetRepository<Child, int>().GetAllAsync();


            var ParentChildren = allChildren
                .Where(c => c.ParentId == ParentId)
                .Select(c => new ChildNameDTO
                {
                    Id = c.Id,     
                    Name = c.Name  
                });

            return ParentChildren;
        }

    }
}

