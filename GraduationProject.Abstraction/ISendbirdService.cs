using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface ISendbirdService
    {
        Task<string> CreateUserAsync(string userId, string nickname);
        Task<string> CreateChannelAsync(string channelName, List<string> userIds);
        Task<string> SendMessageAsync(string channelUrl, string message, string userId);
    }
}
   
