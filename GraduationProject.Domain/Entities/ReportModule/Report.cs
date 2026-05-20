using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.ParentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Data.Entities.ReportModule
{
    public class Report:BaseEntity<int>
    {
        public DateTime AssignedDate { get; set; }
        public string Content { get; set; } = default!;

        public int ChildId { get; set; }
        public Child Child { get; set; } = default!;

        //34an a3raf men l specialist lli 3ml report dah => nav prop
        public int SpecialistId { get; set; }
        public Specialist Specialist { get; set; } = default!;

        public int ParentId { get; set; }       // mandatory
        public Parent Parent { get; set; } = default!;

    }
}
