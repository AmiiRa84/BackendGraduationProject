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
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
        [EmailAddress]
        public string Email { get; set; } = default!;
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
