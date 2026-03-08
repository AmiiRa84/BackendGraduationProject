using GraduationProject.Domain.Data.Entities.TaskModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<SpecialistTask>
    {
        public void Configure(EntityTypeBuilder<SpecialistTask> builder)
        {
            builder.HasKey(t => t.Id);

            #region Properties

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Description)
                   .HasMaxLength(1000);

            builder.Property(t => t.AssignedDate)
                   .IsRequired();

            builder.Property(t => t.DueDate)
                   .IsRequired();

            builder.Property(t => t.TaskStatus)
                   .IsRequired();

            builder.Property(t => t.TaskType)
                   .IsRequired();

            builder.Property(t => t.Source)
                   .IsRequired();
            #endregion



            #region Relationships


            
            builder.HasOne(t => t.Specialist)
       .WithMany(s => s.Tasks)
       .HasForeignKey(t => t.SpecialistId)
       .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Child)
                   .WithMany(c => c.Tasks)
                   .HasForeignKey(t => t.ChildId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.PreDefinedTask)
                   .WithMany()
                   .HasForeignKey(t => t.PredefinedTaskId)
                   .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
