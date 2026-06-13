using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ReportDTOs
{
    public class SaveAvatarReportDto
    {
            public int ChildId { get; set; }
            public string AvatarReport { get; set; } = null!;
        
    }
}
