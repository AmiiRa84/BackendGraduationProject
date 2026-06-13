using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.ReportModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Data.Entities.ChildModule
{
    public class Child:BaseEntity<int>
    {
   
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string Description { get; set; } = default!;
        public int SpecialistId { get; set; }
        public Specialist Specialist { get; set; } = default!;
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = default!;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<SpecialistTask> Tasks { get; set; } = new List<SpecialistTask>();
        public ICollection<AvatarSessionReport> AvatarSessionReports { get; set; } = new List<AvatarSessionReport>();
    }
}
