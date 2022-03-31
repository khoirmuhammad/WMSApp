using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class AuthUserRoleConfiguration : IEntityTypeConfiguration<AuthUserRole>
    {
        public void Configure(EntityTypeBuilder<AuthUserRole> builder)
        {
            builder.ToTable("AuthUserRole");

            builder.HasKey(aur => aur.Id);

            builder.HasOne(aur => aur.AuthUser)
                    .WithMany(aur => aur.AuthUserRole)
                    .HasForeignKey(aur => aur.UserId);

            builder.HasOne(aur => aur.AuthRole)
                    .WithMany(aur => aur.AuthUserRole)
                    .HasForeignKey(aur => aur.RoleId);
        }
    }
}
