using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class TaskResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskStatus { get; set; } = default!;
        public string TaskType { get; set; } = default!;
        public string Source { get; set; } = default!;
        public int ChildId { get; set; }
        public string ChildName { get; set; } = default!;
    }
}
