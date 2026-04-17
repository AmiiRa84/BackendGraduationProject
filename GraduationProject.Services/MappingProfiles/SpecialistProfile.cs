using AutoMapper;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Shared.DTOs.SpecialistDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.MappingProfiles
{
    public class SpecialistProfile :Profile
    {
        public SpecialistProfile()
        {
            CreateMap<Specialist,SpecialistDTO>()
              
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.ToString()));
            CreateMap<UpdateSpecialistDetailsDTO, Specialist>();

            

        }
    }
}
