using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class AsnDetailConfiguration : IEntityTypeConfiguration<AsnDetail>
    {
        public void Configure(EntityTypeBuilder<AsnDetail> builder)
        {
            builder.ToTable("AsnDetail");
            builder.HasKey(ad => ad.Id);
            builder.Property(ad => ad.Id).UseIdentityColumn();
            builder.Property(ad => ad.LineNumber).IsRequired().HasColumnType("tinyint");
            builder.Property(ad => ad.OriginalQty).IsRequired();
            builder.Property(ad => ad.OriginalUnit).IsRequired().HasMaxLength(10);
            builder.Property(ad => ad.ProductCode).IsRequired().HasMaxLength(20);
            builder.Property(ad => ad.ProductName).IsRequired(false).HasMaxLength(100);
            builder.Property(ad => ad.ProductionDate).IsRequired();
            builder.Property(ad => ad.ExpiryDate).IsRequired(false);
            builder.Property(ad => ad.LineStatus).IsRequired().HasMaxLength(20);
            builder.Property(ad => ad.ReceivingArea).HasMaxLength(50);
            builder.Property(ad => ad.Remark).HasMaxLength(100);

            builder.HasOne<Asn>(ad => ad.Asn)
                .WithMany(a => a.AsnDetails)
                .HasForeignKey(ad => ad.AsnCode);
        }
    }
}
