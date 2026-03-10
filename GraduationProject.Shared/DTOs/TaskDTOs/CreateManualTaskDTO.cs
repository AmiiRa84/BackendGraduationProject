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

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public int SpecialistId { get; set; }   // مؤقت لحد JWT
        public int ChildId { get; set; }

        [Required]
        public string TaskType { get; set; } = default!;
    }
}
