using ECommerce.Domain.Entities.SecurityModule;
using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Domain.Entities.ReportModule;
using GraduationProject.Domain.Entities.SecurityModule;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Persistence.IdentityData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.DbContexts
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<SpecialistTask> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TaskResult> TaskResults { get; set; } = default!;
        public DbSet<PreDefinedTask> PreDefinedTasks { get; set; }
        public DbSet<AvatarSessionReport> AvatarSessionReports { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            builder.Entity<PreDefinedTask>()
                .Property(p => p.TaskType)
                .HasConversion<string>();

            builder.Entity<SpecialistTask>()
                .Property(p => p.TaskType)
                .HasConversion<string>();
           builder.Entity<Specialist>()
                .HasOne(s => s.User)           
                .WithOne(u => u.Specialist)    
                .HasForeignKey<Specialist>(s => s.UserId)
                .IsRequired()                
                .OnDelete(DeleteBehavior.NoAction);

         
            builder.Entity<Parent>()
                .HasOne(p => p.User)           
                .WithOne(u => u.Parent)      
                .HasForeignKey<Parent>(p => p.UserId)
                .IsRequired()                
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>()
    .HasOne(r => r.Child)
    .WithMany(c => c.Reports)
    .HasForeignKey(r => r.ChildId)
    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>()
    .HasOne(r => r.Parent)
    .WithMany(r=>r.Reports)
    .HasForeignKey(r => r.ParentId)
    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}