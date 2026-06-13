using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.TaskDTOs
{
    public class SaveTaskResultDto
    {
        public int SpecialistTaskId { get; set; } 
        public int? TotalMoves { get; set; }
        public int? TimeTaken { get; set; }
        public int? RoundsCount { get; set; }
        public string MotherNote { get; set; } = default!;

    }
}
