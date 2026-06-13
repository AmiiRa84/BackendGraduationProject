using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities.ReportModule
{
    public class AvatarSessionReport: BaseEntity<int>
    {
        [ForeignKey("Child")]
        public int ChildId { get; set; }
        public string AvatarReport { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
