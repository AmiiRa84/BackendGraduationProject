using GraduationProject.Shared.DTOs.ChildDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.DashboardDTO
{
    public class ParentWithChildrenDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public List<childDTO02> Children { get; set; } = default!;
    }
}
