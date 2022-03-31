using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            builder.ToTable("AuthUser");

            builder.HasKey(au => au.Id);
            builder.Property(au => au.Name).IsRequired().HasMaxLength(50);
            builder.Property(au => au.Username).IsRequired().HasMaxLength(50);
            builder.Property(au => au.Email).IsRequired().HasMaxLength(100);
            builder.Property(au => au.Password).IsRequired().HasMaxLength(128);
        }
    }
}
