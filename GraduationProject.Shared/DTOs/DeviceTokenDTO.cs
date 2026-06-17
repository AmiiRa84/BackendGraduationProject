using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs
{
    public class DeviceTokenDTO
    {
        public int UserId { get; set; }
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string Platform { get; set; } = default!;//web or mobile
    }
}
