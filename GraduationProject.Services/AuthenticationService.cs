using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.SecurityModule;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.SecurityModule;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs.AuthDTOs;
using GraduationProject.Shared.DTOs.AuthDTOs.Parent;
using GraduationProject.Shared.DTOs.AuthDTOs.Specialist;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GraduationProject.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendbirdService _sendbirdService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ISendbirdService sendbirdService,
            ILogger<AuthenticationService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _sendbirdService = sendbirdService;
            _logger = logger;
        }

        public async Task<ResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.Users
         .Include(u => u.Parent)
             .ThenInclude(p => p!.Children)  
         .Include(u => u.Specialist)
         .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid Email or Password");

            var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkPassword)
                throw new UnauthorizedAccessException("Invalid Email or Password");

            return new ResponseDTO
            {
                Success = true,
                msg = "Login Successful",
                Id = user.Id,
                FrontendId = user.Parent?.Id ?? user.Specialist?.Id,
                Email = user.Email!,
                ChildIds = user.Parent?.Children
            ?.Select(c => c.Id)
            .ToList()
            };
        }

        public async Task<ResponseDTO> RegisterParentAsync(RegisterParentDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                return new ResponseDTO { Success = false, msg = "Email already exists", Email = dto.Email };

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = new Address { City = dto.City, Street = dto.Street, Country = "Egypt" }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new ResponseDTO
                {
                    Success = false,
                    msg = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Email = dto.Email
                };

            await _userManager.AddToRoleAsync(user, "Parent");

            var parent = new Parent { UserId = user.Id };
            var parentRepo = _unitOfWork.GetRepository<Parent, int>();
            await parentRepo.AddAsync(parent);
            await _unitOfWork.SaveChangesAsync();

          
            try { await _sendbirdService.CreateUserAsync($"parent_{parent.Id}", user.FullName); }
            catch (Exception ex) { _logger.LogWarning(ex, "SendBird failed for parent_{Id}", parent.Id); }

            var childRepo = _unitOfWork.GetRepository<Child, int>();
            var specialistRepo = _unitOfWork.GetRepository<Specialist, int>();

            foreach (var childDto in dto.Children)
            {
                var specialists = await specialistRepo.GetAllAsync(q => q.Include(s => s.Childs));

                var assignedSpecialist = specialists
                    .OrderBy(s => s.Childs.Count)
                    .FirstOrDefault()
                    ?? throw new ArgumentException("No specialists available at the moment");

                var child = new Child
                {
                    Name = childDto.Name,
                    Age = childDto.Age,
                    Description = childDto.Description,
                    ParentId = parent.Id,
                    SpecialistId = assignedSpecialist.Id
                };

                await childRepo.AddAsync(child);
                await _unitOfWork.SaveChangesAsync();

                // ✅ اعمل Chat بين الـ Parent والـ Specialist بتاع كل Child
                try
                {
                    await _sendbirdService.CreateChatBetweenParentAndSpecialistAsync(
                        parent.Id,
                        assignedSpecialist.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "SendBird chat creation failed for parent_{PId} & specialist_{SId}",
                        parent.Id, assignedSpecialist.Id);
                }
            }

            return new ResponseDTO
            {
                Success = true,
                msg = $"Parent registered successfully with {dto.Children.Count} child(ren)",
                Id = user.Id,
                FrontendId = parent.Id,     
                Email = dto.Email
            };
        }

        public async Task<ResponseDTO> RegisterSpecialistAsync(RegisterSpecialistDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                return new ResponseDTO { Success = false, msg = "Registration failed: email already exists", Email = dto.Email };

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = new Address { City = dto.City, Street = dto.Street, Country = "Egypt" }
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new ResponseDTO
                {
                    Success = false,
                    msg = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Email = dto.Email
                };

            await _userManager.AddToRoleAsync(user, "Specialist");

            var specialist = new Specialist { UserId = user.Id };
            var specialistRepo = _unitOfWork.GetRepository<Specialist, int>();
            await specialistRepo.AddAsync(specialist);
            await _unitOfWork.SaveChangesAsync();

            // SendBird - فشله مش بيوقف الـ Registration
            try { await _sendbirdService.CreateUserAsync($"specialist_{specialist.Id}", user.FullName); }
            catch (Exception ex) { _logger.LogWarning(ex, "SendBird failed for specialist_{Id}", specialist.Id); }

            return new ResponseDTO
            {
                Success = true,
                msg = $"Specialist with id {specialist.Id} registered successfully",
                Id = user.Id,
                FrontendId = specialist.Id,   // ← ده اللي ناقص
                Email = dto.Email
            };
        }

        public async Task DeleteSpecialistAsync(int id)
        {
            var specialistRepo = _unitOfWork.GetRepository<Specialist, int>();
            var specialists = await specialistRepo.GetAllAsync(
                q => q.Include(s => s.Childs).Include(s => s.User));

            var specialist = specialists.FirstOrDefault(s => s.Id == id)
                ?? throw new SpecialistNotFoundException(id);

            if (specialist.Childs.Any())
                throw new ArgumentException("Cannot delete specialist with assigned children. Please reassign children first.");

            specialistRepo.Delete(specialist);
            await _unitOfWork.SaveChangesAsync();
            await _userManager.DeleteAsync(specialist.User);
        }

        public async Task UpdateSpecialistAsync(int id, UpdateUserDTO dto)
        {
            var specialistRepo = _unitOfWork.GetRepository<Specialist, int>();
            var specialist = await specialistRepo.GetByIdAsync(id)
                ?? throw new SpecialistNotFoundException(id);

            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == specialist.UserId)
                ?? throw new SpecialistNotFoundException(id);

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.Phone;
            user.Address.City = dto.City;
            user.Address.Street = dto.Street;

            await _userManager.UpdateAsync(user);
        }

        public async Task<ResponseDTO> UpdateParentAsync(int id, UpdateUserDTO dto)
        {
            var parentRepo = _unitOfWork.GetRepository<Parent, int>();
            var parent = await parentRepo.GetByIdAsync(id)
                ?? throw new ParentNotFoundException(id);

            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == parent.UserId)
                ?? throw new ParentNotFoundException(id);

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.Phone;
            user.Address.City = dto.City;
            user.Address.Street = dto.Street;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseDTO { Success = true, msg = "Parent updated successfully", Email = user.Email! };
        }

        public async Task<ResponseDTO> DeleteParentAsync(int id)
        {
            var parentRepo = _unitOfWork.GetRepository<Parent, int>();
            var parents = await parentRepo.GetAllAsync(q => q.Include(p => p.Children));
            var parent = parents.FirstOrDefault(p => p.Id == id)
                ?? throw new ArgumentException("Data inconsistency: parent exists but user not found");

            var user = await _userManager.FindByIdAsync(parent.UserId!)
                ?? throw new ArgumentException("Data inconsistency: parent exists but user not found");

            var childRepo = _unitOfWork.GetRepository<Child, int>();
            foreach (var child in parent.Children)
                childRepo.Delete(child);

            await _unitOfWork.SaveChangesAsync();

            parentRepo.Delete(parent);
            await _unitOfWork.SaveChangesAsync();
            await _userManager.DeleteAsync(user);

            return new ResponseDTO { Success = true, msg = "Parent deleted successfully" };
        }

        public async Task<ResponseDTO> VerifyChildModePasswordAsync(
     int parentId,
     VerifyChildModePasswordDTO dto)
        {
            var parentRepo = _unitOfWork.GetRepository<Parent, int>();

            var parent = await parentRepo.GetByIdAsync(parentId)
                ?? throw new ParentNotFoundExceptionAuth();

            var user = await _userManager.FindByIdAsync(parent.UserId!)
                ?? throw new ParentNotFoundExceptionAuth();

            var isCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isCorrect)
            {
                return new ResponseDTO
                {
                    Success = false,
                    msg = "Incorrect password"
                };
            }

            return new ResponseDTO
            {
                Success = true,
                msg = "Password verified. Exiting child mode."
            };
        }
    }
    }