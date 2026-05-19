using GraduationProject.Shared.DTOs.ChildDTOs;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Shared.DTOs.AuthDTOs.Parent
{
    public class RegisterParentDTO
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
          ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^01[0125][0-9]{8}$",
          ErrorMessage = "Phone number must be a valid Egyptian number")]
        public string PhoneNumber { get; set; } = default!;

        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public List<ChildDTO> Children { get; set; } = new();
    }
}