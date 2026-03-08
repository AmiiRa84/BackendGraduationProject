using GraduationProject.Domain.Data.Entities.ChildModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.Configurations
{
    public class ChildConfigurations : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            builder.HasOne(c => c.Parent)
        .WithMany(p => p.Children)
        .HasForeignKey(c => c.ParentId)
        .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Specialist)
                   .WithMany(s => s.Childs)
                   .HasForeignKey(c => c.SpecialistId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
