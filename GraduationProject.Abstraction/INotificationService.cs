using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface INotificationService
    {
        Task SaveDeviceTokenAsync(int frontendId, string role, string token, string platform);

        Task SendAsync(string deviceToken, string title, string body, string type);

        Task SendToUserAsync(string userId, string title, string body, string type);
    
}
}
