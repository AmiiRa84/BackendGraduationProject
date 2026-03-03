using GraduationProject.Domain.Data.Entities.ChildModule;
using GraduationProject.Domain.Data.Entities.ReportModule;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Data.Entities.TaskModule;
using GraduationProject.Domain.Entities.TaskModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.DbContexts
{
    public class StoreDbContext : DbContext
    {
        
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PreDefinedTask>()
                .Property(p => p.TaskType)
                .HasConversion<string>();

            modelBuilder.Entity<SpecialistTask>()
                .Property(p => p.TaskType)
                .HasConversion<string>();
        }
        
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }
 

        public DbSet<Specialist> Specialists { get; set; } = default!;
        public DbSet<SpecialistTask> Tasks { get; set; } = default!;
        public DbSet<Report> Reports { get; set; } = default!;
        public DbSet<Child> Children { get; set; } = default!;
        public DbSet<PreDefinedTask> preDefinedTasks { get; set; }
    }
}
