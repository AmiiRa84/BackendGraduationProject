using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.SecurityModule;
using GraduationProject.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.SecurityModule;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataSeed
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityDataInitializer> _logger;
        private readonly StoreDbContext _context; 

        public IdentityDataInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<IdentityDataInitializer> logger,
            StoreDbContext context) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task InitializeDataAsync()
        {
            try
            {
                var hasRoles = await _roleManager.Roles.AnyAsync();
                if (!hasRoles)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Specialist"));
                    await _roleManager.CreateAsync(new IdentityRole("Parent"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var hasUsers = await _userManager.Users.AnyAsync();
                if (!hasUsers)
                {
                    var specialists = new[]
                    {
                new { FullName = "Dr Lilly",   Email = "ahmed@spec.com",   Phone = "01000000001", UserName = "lilly02",   City = "Cairo", Street = "Maadi" },
                new { FullName = "Dr Amira",   Email = "amira@spec.com",   Phone = "01000000002", UserName = "amira1",   City = "Cairo", Street = "Heliopolis" },
                new { FullName = "Dr Ali",     Email = "ali@spec.com",     Phone = "01000000003", UserName = "ali1",     City = "Giza",  Street = "Dokki" },
                new { FullName = "Dr Sara",    Email = "sara@spec.com",    Phone = "01000000004", UserName = "sara1",    City = "Giza",  Street = "Mohandseen" },
                new { FullName = "Dr Omar",    Email = "omar@spec.com",    Phone = "01000000005", UserName = "omar1",    City = "Alex",  Street = "Stanley" },
                new { FullName = "Dr Laila",   Email = "laila@spec.com",   Phone = "01000000006", UserName = "laila1",   City = "Alex",  Street = "Smouha" },
                new { FullName = "Dr Youssef", Email = "youssef@spec.com", Phone = "01000000007", UserName = "youssef1", City = "Cairo", Street = "Maadi" },
                new { FullName = "Dr Dina",    Email = "dina@spec.com",    Phone = "01000000008", UserName = "dina1",    City = "Cairo", Street = "Zamalek" },
                new { FullName = "Dr Karim",   Email = "karim@spec.com",   Phone = "01000000009", UserName = "karim1",   City = "Giza",  Street = "Haram" },
                new { FullName = "Dr Hania",   Email = "hania@spec.com",   Phone = "01000000010", UserName = "hania6",   City = "Cairo", Street = "New cairo" },
            };

                    foreach (var s in specialists)
                    {
                        var user = new ApplicationUser
                        {
                            FullName = s.FullName,
                            Email = s.Email,
                            UserName = s.UserName,
                            PhoneNumber = s.Phone,
                            Address = new Address { City = s.City, Street = s.Street, Country = "Egypt" }
                        };

                        var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                                _logger.LogError($"Error creating specialist {s.UserName}: {error.Description}");
                            continue;
                        }

                        await _userManager.AddToRoleAsync(user, "Specialist");
                        await _context.Specialists.AddAsync(new Specialist {  UserId = user.Id });
                        await _context.SaveChangesAsync(); 
                    }

                    var parents = new[]
                    {
                new { FullName = "Lilly",  Email = "lilly@mail.com",  Phone = "01120000001", UserName = "lilly" },
                new { FullName = "Mila",   Email = "mila@mail.com",   Phone = "01120000002", UserName = "mila" },
                new { FullName = "Hazem",  Email = "hazem@mail.com",  Phone = "01120000003", UserName = "hazem" },
                new { FullName = "Sara",   Email = "sara@mail.com",   Phone = "01120000004", UserName = "sara" },
                new { FullName = "Maya",   Email = "maya@mail.com",   Phone = "01120000005", UserName = "maya" },
                new { FullName = "Dalia",  Email = "dalia@mail.com",  Phone = "01120000006", UserName = "dalia" },
                new { FullName = "Nour",   Email = "nour@mail.com",   Phone = "01120000007", UserName = "nour" },
                new { FullName = "Tarek",  Email = "tarek@mail.com",  Phone = "01120000008", UserName = "tarek" },
                new { FullName = "Rania",  Email = "rania@mail.com",  Phone = "01120000009", UserName = "rania" },
                new { FullName = "Khaled", Email = "khaled@mail.com", Phone = "01120000010", UserName = "khaled" },
            };

                    foreach (var p in parents)
                    {
                        var user = new ApplicationUser
                        {
                            FullName = p.FullName,
                            Email = p.Email,
                            UserName = p.UserName,
                            PhoneNumber = p.Phone,
                            Address = new Address { City = "Cairo", Street = "Unknown", Country = "Egypt" }
                        };

                        var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                                _logger.LogError($"Error creating parent {p.UserName}: {error.Description}");
                            continue;
                        }

                        await _userManager.AddToRoleAsync(user, "Parent");
                        await _context.Parents.AddAsync(new Parent { UserId = user.Id });
                        await _context.SaveChangesAsync(); // ✅ هنا
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while seeding Database, {ex.Message} happened");
            }
        }
    }
    }
