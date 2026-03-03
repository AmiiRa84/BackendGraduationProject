using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.TaskModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Data.Entities.TaskModule
{
    public class SpecialistTask:BaseEntity<int>
    {
        public string Title { get; set; } = default!;
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; } = default!;
        public TStatus TaskStatus { get; set; }
        public TaskType TaskType { get; set; }
        public TaskSource Source { get; set; } //ltask hykon pre-defined wla ai wla custom
       
        public int SpecialistId { get; set; }
        public Specialist Specialist { get; set; } = default!;

        public int? PredefinedTaskId { get; set; }
        public PreDefinedTask? PreDefinedTask { get; set; }

        public int ParentId { get; set; }   
        public Parent Parent { get; set; } = default!;
    }
}

