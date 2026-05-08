using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChildDTOs
{
    public class ChildProgressDTO
    {
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string Description { get; set; } = default!;
        public string ParentName { get; set; } = default!;

    }
}
