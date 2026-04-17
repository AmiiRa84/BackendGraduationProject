using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChildDTOs
{
    public class ChildTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string TaskStatus { get; set; } = default!; // تحويل الـ Enum لـ string
        public DateTime DueDate { get; set; }
        public DateTime AssignedDate { get; set; }
     }
}
