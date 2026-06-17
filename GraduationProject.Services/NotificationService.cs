using ECommerce.Domain.Contracts;
using FirebaseAdmin.Messaging;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.SecurityModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task SaveDeviceTokenAsync(int frontendId, string role, string token, string platform)
        {
            string? userId;
            if (role.Equals("Parent", StringComparison.OrdinalIgnoreCase))
            {
                var parent = await _unitOfWork.GetRepository<Parent, int>().GetByIdAsync(frontendId)
                    ?? throw new ParentNotFoundException(frontendId);
                userId = parent.UserId;
            }
            else
            {
                var specialist = await _unitOfWork.GetRepository<Specialist, int>().GetByIdAsync(frontendId)
                    ?? throw new SpecialistNotFoundException(frontendId);
                userId = specialist.UserId;
            }

            var tokenRepo = _unitOfWork.GetRepository<UserDeviceToken, int>();
            var existing = (await tokenRepo.GetAllAsync(q =>
                q.Where(t => t.UserId == userId && t.Platform == platform)))
                .FirstOrDefault();

            if (existing != null)
                existing.Token = token;
            else
                await tokenRepo.AddAsync(new UserDeviceToken
                {
                    UserId = userId!,
                    Token = token,
                    Platform = platform
                });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendToUserAsync(string userId, string title, string body, string type)
        {
            Console.WriteLine($"Sending To UserId = {userId}");

            var tokens = await _unitOfWork.GetRepository<UserDeviceToken, int>()
                .GetAllAsync(q => q.Where(t => t.UserId == userId));

            foreach (var deviceToken in tokens)
            {
                try
                {
                    await SendAsync(deviceToken.Token, title, body, type);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task SendAsync(string deviceToken, string title, string body, string type)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification() { Title = title, Body = body },
                Data = new Dictionary<string, string> { { "type", type } }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}