using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities;
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
    }
}
