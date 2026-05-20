using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ReportDTOs
{
    public class GenerateReportRequestDto
    {
        public int ChildId { get; set; }
        public int? TotalMovesToAchiveGoal { get; set; }
        public int? TimeTakenToAchiveGoal { get; set; }
        public int? RoundsCount { get; set; }
        public string MotherNote { get; set; } = string.Empty;
    }
}
