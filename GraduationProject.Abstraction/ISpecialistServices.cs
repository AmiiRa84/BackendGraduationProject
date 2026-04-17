using GraduationProject.Shared.DTOs.ParentDTOs;
using GraduationProject.Shared.DTOs.SpecialistDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface ISpecialistServices
    {
        Task<SpecialistDTO> GetDetailsAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateSpecialistDetailsDTO dto);

       

    }
}
