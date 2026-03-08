using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class TaskTitleAndStatusDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string TaskStatus { get; set; } = default!;//completed , pending 
    }
}
