using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Shared.DTOs.ChildDTOs;
namespace GraduationProject.Services.MappingProfiles
{
    public class ChildProfile : Profile
    {
        public ChildProfile()
        {
            
            CreateMap<Child, ChildDTO>();
        }
    }
}
