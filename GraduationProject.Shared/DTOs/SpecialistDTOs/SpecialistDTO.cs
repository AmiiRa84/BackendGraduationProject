using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.SpecialistDTOs
{
    public class SpecialistDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;

    }
}
