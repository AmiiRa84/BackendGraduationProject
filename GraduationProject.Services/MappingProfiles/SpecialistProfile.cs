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
       //     CreateMap<Specialist, SpecialistDTO>()
       //.ForMember(dest => dest.Name,
       //    opt => opt.MapFrom(src => src.User.FullName))
       //.ForMember(dest => dest.Email,
       //    opt => opt.MapFrom(src => src.User.Email))
       //.ForMember(dest => dest.Username,
       //    opt => opt.MapFrom(src => src.User.UserName))
       //.ForMember(dest => dest.Phone,
       //    opt => opt.MapFrom(src => src.User.PhoneNumber))
       //.ForMember(dest => dest.City,
       //    opt => opt.MapFrom(src => src.User.Address.City))
       //.ForMember(dest => dest.Street,
       //    opt => opt.MapFrom(src => src.User.Address.Street));

              
                
            CreateMap<UpdateSpecialistDetailsDTO, Specialist>();

            

        }
    }
}
