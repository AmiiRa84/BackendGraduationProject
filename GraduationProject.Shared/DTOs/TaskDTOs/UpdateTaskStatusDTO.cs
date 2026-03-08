using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class UpdateTaskStatusDTO
    {
        public int TaskId { get; set; }
        public string Status { get; set; } = default!;

    }
}
