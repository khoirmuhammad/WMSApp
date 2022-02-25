using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WMSApplication.Models.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(p => p.Code);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.WholeUnit).IsRequired().HasMaxLength(10);
            builder.Property(p => p.LooseQty).IsRequired();
            builder.Property(p => p.WholeQty).IsRequired();
            builder.Property(p => p.AllocationType).IsRequired().HasMaxLength(10);
            builder.Property(p => p.LoosePrice).IsRequired();
            builder.Property(p => p.WholePrice).IsRequired();

            // one product category can be used by product in many data
            // product category : Food (one)
            // product : indomie, roti'o, cake (many). They have food category
            builder.HasOne<ProductCategory>(pc => pc.ProductCategory)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryCode);

            builder.HasOne<Unit>(u => u.Unit)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.UnitCode);
        }
    }
}
