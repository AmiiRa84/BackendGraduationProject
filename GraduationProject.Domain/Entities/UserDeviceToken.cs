using GraduationProject.Domain.Entities.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities
{
    public class UserDeviceToken: BaseEntity<int>
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string Platform { get; set; } = default!; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
