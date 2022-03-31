using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class AuthRoleConfiguration : IEntityTypeConfiguration<AuthRole>
    {
        public void Configure(EntityTypeBuilder<AuthRole> builder)
        {
            builder.ToTable("AuthRole");

            builder.HasKey(ar => ar.Id);
            builder.Property(ar => ar.Name).IsRequired().HasMaxLength(50);
        }
    }
}
