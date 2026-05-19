using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.AuthDTOs.Specialist
{
    public class RegisterSpecialistDTO
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
        //FullName = dto.FullName,
        //        Email = dto.Email,
        //        UserName = dto.Email,
        //        PhoneNumber = dto.PhoneNumber,
        //        Address = new Address { City = dto.City, Street = dto.Street, Country = "Egypt" }
    }
}
