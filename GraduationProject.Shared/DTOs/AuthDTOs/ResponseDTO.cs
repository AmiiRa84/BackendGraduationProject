using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.AuthDTOs
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public string msg { get; set; } = default!;
        public int? FrontendId { get; set; }
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<int>? ChildIds { get; set; } = new();
    

    }
}
