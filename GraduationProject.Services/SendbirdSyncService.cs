using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class SendbirdSyncService:ISendbirdSyncService
    {
        private readonly ISendbirdService _sendbirdService;
        private readonly IUnitOfWork _unitOfWork;

        public SendbirdSyncService(ISendbirdService sendbirdService, IUnitOfWork unitOfWork)
        {
            _sendbirdService = sendbirdService;
            _unitOfWork = unitOfWork;
        }
        public async Task SyncAllUsersAsync()
        {
            var parentRepo = _unitOfWork.GetRepository<Parent, int>();
            var specialistRepo = _unitOfWork.GetRepository<Specialist, int>();

            var parentsFromDb = await parentRepo.GetAllAsync();
            var specialistsFromDb = await specialistRepo.GetAllAsync();

            foreach (var parent in parentsFromDb)
            {
                await _sendbirdService.CreateUserAsync($"parent_{parent.Id}", parent.User.FullName);
            }

            foreach (var specialist in specialistsFromDb)
            {
                await _sendbirdService.CreateUserAsync($"specialist_{specialist.Id}", specialist.User.FullName);
            }
        }

    }
}
