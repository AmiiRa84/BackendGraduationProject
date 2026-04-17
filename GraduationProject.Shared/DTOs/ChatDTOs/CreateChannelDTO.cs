using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChatDTOs
{
    public class CreateChannelDTO
    {
        public string ChannelName { get; set; } = default!;
        public List<string> UserIds { get; set; } = new List<string>();
    }
}
