using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class CreateManualTaskDTO
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TaskType { get; set; }

        public int? PredefinedTaskId { get; set; }  // لو اختار predefined

        [DataType(DataType.Date)]
        [Required] public DateTime DueDate { get; set; }
        public int SpecialistId { get; set; }
        public int ChildId { get; set; }
    }
}
