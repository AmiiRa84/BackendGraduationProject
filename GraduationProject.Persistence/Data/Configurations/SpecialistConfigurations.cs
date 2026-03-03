using GraduationProject.Domain.Data.Entities.SpecialistModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.Configurations
{
   public class SpecialistConfigurations : IEntityTypeConfiguration<Specialist>
    {
        public void Configure(EntityTypeBuilder<Specialist> builder)
        {
            // Primary Key
            #region Configurations
            builder.HasKey(u => u.Id);


            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Phone)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired();



            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(50);


            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(200);


            builder.Property(u => u.City)
                   .IsRequired()
                   .HasMaxLength(10);


            builder.Property(u => u.Street)
                   .IsRequired()
                   .HasMaxLength(150); 
            #endregion


            #region RelationShips

            builder.HasMany(u => u.Reports)
                .WithOne(u => u.Specialist)
                .HasForeignKey(u => u.SpecialistId);


            builder.HasMany(u => u.Tasks)
                   .WithOne(s => s.Specialist)
                   .HasForeignKey(t => t.SpecialistId); 
            #endregion



        }
    }
}
