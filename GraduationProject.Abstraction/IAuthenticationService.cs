using GraduationProject.Shared.DTOs.AuthDTOs;
using GraduationProject.Shared.DTOs.AuthDTOs.Parent;
using GraduationProject.Shared.DTOs.AuthDTOs.Specialist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{

        public interface IAuthenticationService
        {
        Task<ResponseDTO> LoginAsync(LoginDTO dto);

        // REGISTER
        Task<ResponseDTO> RegisterParentAsync(RegisterParentDTO dto);
        Task<ResponseDTO> RegisterSpecialistAsync(RegisterSpecialistDTO dto);

        // SPECIALIST
        Task DeleteSpecialistAsync(int id);
        Task UpdateSpecialistAsync(int id, UpdateUserDTO dto);

        // PARENT
        Task<ResponseDTO> UpdateParentAsync(int id, UpdateUserDTO dto);
        Task<ResponseDTO> DeleteParentAsync(int id);

        // CHILD MODE
       Task<ResponseDTO> VerifyChildModePasswordAsync(int parentId, VerifyChildModePasswordDTO dto);
    }
    }
