using AutoMapper;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Shared.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.MappingProfiles
{
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<PreDefinedTask, PredefinedTaskDTO>()
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType.ToString()));

            CreateMap<SpecialistTask, TaskTitleAndStatusDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()));



        }
    }
}
