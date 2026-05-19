using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class SendbirdSyncService : ISendbirdSyncService
    {
        private readonly ISendbirdService _sendbirdService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendbirdSyncService> _logger;

        public SendbirdSyncService(
            ISendbirdService sendbirdService,
            IUnitOfWork unitOfWork,
            ILogger<SendbirdSyncService> logger)
        {
            _sendbirdService = sendbirdService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

       
        public async Task SyncAllUsersAsync()
        {
          
            var parents = await _unitOfWork
                .GetRepository<Parent, int>()
                .GetAllAsync(q => q.Include(p => p.User));

           
            var specialists = await _unitOfWork
                .GetRepository<Specialist, int>()
                .GetAllAsync(q => q.Include(s => s.User));

          
            foreach (var parent in parents)
            {
                try
                {
                    await _sendbirdService.CreateUserAsync(
                        $"parent_{parent.Id}",
                        parent.User.FullName);
                }
                catch (Exception ex)
                {
                  
                    _logger.LogWarning(ex,
                        "SendBird failed for parent_{Id}", parent.Id);
                }
            }

           
            foreach (var specialist in specialists)
            {
                try
                {
                    await _sendbirdService.CreateUserAsync(
                        $"specialist_{specialist.Id}",
                        specialist.User.FullName);
                }
                catch (Exception ex)
                {
                
                    _logger.LogWarning(ex,
                        "SendBird failed for specialist_{Id}", specialist.Id);
                }
            }
        }
    }
}