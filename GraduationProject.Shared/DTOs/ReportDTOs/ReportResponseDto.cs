using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ReportDTOs
{
    public class ReportResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string SpecialistName { get; set; } = string.Empty;
    }
}
