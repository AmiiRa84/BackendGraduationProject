using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.AuthDTOs.Parent
{
    public class VerifyChildModePasswordDTO
    {
        public string Password { get; set; } = default!;
        public string UsserId { get; set; } = default!;
    }
}
