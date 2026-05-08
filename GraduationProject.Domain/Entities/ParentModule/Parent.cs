using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities.ParentModule
{
    public class Parent :BaseEntity<int>
    {
   
        public ApplicationUser User { get; set; } = default!;
        public string? UserId { get; set; }
        public ICollection<Child> Children { get; set; } = new List<Child>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
     //   public ICollection<SpecialistTask> Tasks { get; set; } = new List<SpecialistTask>();

    }
}
