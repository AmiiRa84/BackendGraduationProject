using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.ChatDTOs
{
    public class SendMessageDTO
    {
        public string ChannelUrl { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
