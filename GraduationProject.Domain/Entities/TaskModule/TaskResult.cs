using GraduationProject.Domain.Data.Entities.TaskModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities.TaskModule
{
    public class TaskResult : BaseEntity<int>
    {
        public int? TotalMoves { get; set; }
        public int? TimeTaken { get; set; }
        public int? RoundsCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int SpecialistTaskId { get; set; }
        public SpecialistTask SpecialistTask { get; set; } = default!;
    }
}
