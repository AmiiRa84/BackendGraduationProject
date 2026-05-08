using AutoMapper;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.MappingProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<PreDefinedTask, PredefinedTaskDTO>()
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType.ToString()));

            CreateMap<SpecialistTask, TaskTitleAndStatusDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()));

            CreateMap<SpecialistTask, TasksDueTodayDTO>()
     .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.Name))
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.ToString()));

            CreateMap<SpecialistTask, ChildTaskDTO>();
            // SpecialistTask => CreateManualTaskDTO
            CreateMap<SpecialistTask, CreateManualTaskDTO>()
       .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType.ToString()))
       .ForMember(dest => dest.PredefinedTaskId, opt => opt.MapFrom(src => src.PredefinedTaskId))
       // Law el-Task fiha property esmaha Child, khally el-mapping yrooh yegeeb el-ID menha
       .ForMember(dest => dest.SpecialistId, opt => opt.MapFrom(src => src.Child.SpecialistId));

            CreateMap<SpecialistTask, TaskResponseDTO>()
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType.ToString()))
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.ToString()))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.Name)); // لو عندك Child
        
        }
    }
}
