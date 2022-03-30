using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(l => l.Code);
            builder.Property(l => l.Floor).HasColumnType("tinyint").IsRequired();
            builder.Property(l => l.RackAisle).IsRequired().HasMaxLength(2);
            builder.Property(l => l.Level).HasColumnType("tinyint").IsRequired();
            builder.Property(l => l.Pos).HasColumnType("tinyint").IsRequired();
            builder.Property(l => l.Type).IsRequired().HasMaxLength(15);
            builder.Property(l => l.MaximumPallet).HasColumnType("tinyint").IsRequired();
        }
    }
}
