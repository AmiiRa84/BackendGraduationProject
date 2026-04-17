using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities;
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
        public string Name { get; set; } = default!;
        public int Phone { get; set; }
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
       
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;

        //public ICollection<SpecialistTask> Tasks { get; set; } = default!;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        //public ICollection<Child> Childs { get; set; } = new List<Child>();
       // public ICollection<Child> Childs { get; set; } = new HashSet<Child>();
        public ICollection<Child> Childs { get; set; } = new LinkedList<Child>();

    }



}
