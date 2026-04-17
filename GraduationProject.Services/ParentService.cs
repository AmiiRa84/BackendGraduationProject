using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ParentDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class ParentService : IParentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParentService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ParentGetNameDTO>> GetParentsBySpecialistIdAsync(int specialistId)
        {
            var parents = await _unitOfWork.GetRepository<Parent, int>()
                .GetAllAsync(q => q
                    .Where(p => p.Children.Any(c => c.SpecialistId == specialistId))
                    .Include(p => p.Children)
                );


            return _mapper.Map<IEnumerable<ParentGetNameDTO>>(parents);
        }
    }
}
