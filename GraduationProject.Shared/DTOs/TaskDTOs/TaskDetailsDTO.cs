using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class TaskDetailsDTO
    {


        public int TaskId { get; set; }
        public string Title { get; set; }

        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Description { get; set; } = default!;
        public double PlannedDays { get; set; }
        public double? ActualDays { get; set; }
        public string Status { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public string PunctualityStatus { get; set; } = default!;
    }
    }

