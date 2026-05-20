using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ReportDTOs
{
    public class FinalReportResponseDto
    {
        public string ChildName { get; set; } = string.Empty;
        public string MotherNote { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
