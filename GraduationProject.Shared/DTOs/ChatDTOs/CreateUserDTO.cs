using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChatDTOs
{
    public class CreateUserDTO
    {
        public string UserId { get; set; } = default!;
        public string Nickname { get; set; } = default!;
    }
}
