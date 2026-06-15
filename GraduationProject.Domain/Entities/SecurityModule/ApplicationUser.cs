using ECommerce.Domain.Entities.SecurityModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Domain.Entities.SecurityModule
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public Address Address { get; set; } = default!;

        
        public Specialist? Specialist { get; set; }
        public Parent? Parent { get; set; }



    }
}
