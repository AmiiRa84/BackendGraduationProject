using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChildDTOs
{
    public class ChildProgressDTO
    {
        public string? SpecialistName { get; set; }
        public int SpecialistId { get; set; }
        public string? SpecialistEmail { get; set; }
        public string? ParentPhone { get; set; } 
        public string? ParentEmail { get; set; }
        public string ChildName { get; set; } = default!;
        public int Age { get; set; }
        public string? Description { get; set; } 
        public string? ParentName { get; set; } = default!;

    }
}
