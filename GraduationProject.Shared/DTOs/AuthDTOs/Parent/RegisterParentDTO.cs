using GraduationProject.Shared.DTOs.ChildDTOs;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Shared.DTOs.AuthDTOs.Parent
{
    public class RegisterParentDTO
    {
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public List<ChildDTO> Children { get; set; } = new();
    }
}