using GraduationProject.Shared.DTOs.ParentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface IParentService
    {

       
        Task<IEnumerable<ParentGetNameDTO>> GetParentsBySpecialistIdAsync(int specialistId);
    }
}
