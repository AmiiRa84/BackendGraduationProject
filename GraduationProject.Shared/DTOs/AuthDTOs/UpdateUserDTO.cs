using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.AuthDTOs
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
