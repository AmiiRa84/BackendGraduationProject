using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.DashboardDTO
{
    public class childDTO02
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string Description { get; set; } = default!;

        public List<TaskDTO02> Tasks { get; set; } = default!;
    }
}
