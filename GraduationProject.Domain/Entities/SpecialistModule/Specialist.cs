using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.SecurityModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Data.Entities.SpecialistModule
{
    public class Specialist:BaseEntity<int>
    {
       
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
      
        public ICollection<Child> Childs { get; set; } = new LinkedList<Child>();


    }



}
