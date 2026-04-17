using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs.SpecialistDTOs
{
    public class UpdateSpecialistDetailsDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone must start with 010, 011, 012, or 015 and be 11 digits long")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters")]
        public string Username { get; set; } = default!;

       

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; } = default!;
    }
}
