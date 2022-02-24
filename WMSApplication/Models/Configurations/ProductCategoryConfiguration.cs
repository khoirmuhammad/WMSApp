using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory");

            builder.HasKey(pc => pc.Id);
            builder.Property(pc => pc.Code).IsUnicode().IsRequired().HasMaxLength(10);
            builder.Property(pc => pc.Name).IsRequired().HasMaxLength(50);
            builder.Property(pc => pc.Description).HasMaxLength(100);
        }
    }
}
