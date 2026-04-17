using GraduationProject.Shared.DTOs.ChildDTOs;
using GraduationProject.Shared.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Abstraction
{
    public interface IChildServices
    {
        Task<ChildProgressDTO> GetChildProgressAsync(int childId);
        Task<IEnumerable<ChildDTO>> GetChildrenBySpecialistIdAsync(int SpecialistId);
        Task<IEnumerable<ChildNameDTO>> GetChildrenByParentIdAsync(int ParentId);
    }
}
