using AutoMapper;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Shared.DTOs.ParentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.MappingProfiles
{
    public class ParentProfile :Profile
    {
        public ParentProfile()
        {
            CreateMap<Parent, ParentGetNameDTO>()
     .ForMember(dest => dest.name
     , opt => opt.MapFrom(src => src.User.FullName));

        }
    }
}
