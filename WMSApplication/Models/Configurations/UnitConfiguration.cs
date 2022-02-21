using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Unit");

            builder.HasKey(u => u.Id);
            builder.Property( u=> u.Name).IsRequired().HasMaxLength(10);
            builder.Property(u => u.Description).IsRequired().HasMaxLength(50);
        }
    }
}
