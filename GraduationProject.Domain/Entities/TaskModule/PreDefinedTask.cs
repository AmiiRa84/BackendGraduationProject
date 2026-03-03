using GraduationProject.Domain.Data.Entities.TaskModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities.TaskModule
{
    public class PreDefinedTask:BaseEntity<int>
    {

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        public TaskType TaskType { get; set; }

        public string? AvatarTopic { get; set; }
    }
}
