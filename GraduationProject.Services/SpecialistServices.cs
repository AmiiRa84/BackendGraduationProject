using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ParentDTOs;
using GraduationProject.Shared.DTOs.SpecialistDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class SpecialistServices : ISpecialistServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecialistServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<SpecialistDTO> GetDetailsAsync(int id)
        {
            var specialistDetails =await _unitOfWork.GetRepository<Specialist,int>().GetByIdAsync(id);
           return _mapper.Map<SpecialistDTO>(specialistDetails);


        }

   

        public async Task<bool> UpdateAsync(int id, UpdateSpecialistDetailsDTO dto)
        {
            var specialist = await _unitOfWork.GetRepository<Specialist, int>()
                                         .GetByIdAsync(id);
            if (specialist == null) return false;

            _mapper.Map(dto,specialist);

            if (!int.TryParse(dto.Phone, out int phoneInt))
                throw new Exception("Phone number invalid");

            specialist.Phone = phoneInt;
            specialist.Phone = int.Parse(dto.Phone);
            _unitOfWork.GetRepository<Specialist, int>().Update(specialist);
            await _unitOfWork.SaveChangesAsync(); 
            return true;
        }
    }
}
