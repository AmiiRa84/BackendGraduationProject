using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
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
            var specialist = await _unitOfWork
     .GetRepository<Specialist, int>()
     .GetAllAsync(q => q
         .Where(s => s.Id == id)
         .Include(s => s.User)
         .ThenInclude(u => u.Address));


            var result = specialist.FirstOrDefault();

            if (result is null) 
                throw new SpecialistNotFoundException(id);
            return new SpecialistDTO
            {
                Id = result.Id,
                Name = result.User.FullName,
                Email = result.User.Email,
                Username = result.User.UserName,
                Phone = result.User.PhoneNumber,
                City = result.User.Address.City,
                Street = result.User.Address.Street
            };
        }

   

        public async Task<bool> UpdateAsync(int id, UpdateSpecialistDetailsDTO dto)
        {
            var specialist = await _unitOfWork.GetRepository<Specialist, int>()
                                         .GetByIdAsync(id);
            if (specialist == null) return false;

            _mapper.Map(dto,specialist);



            specialist.User.PhoneNumber = dto.Phone;
           
            _unitOfWork.GetRepository<Specialist, int>().Update(specialist);
            await _unitOfWork.SaveChangesAsync(); 
            return true;
        }
    }
}
