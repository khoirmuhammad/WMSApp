using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class AsnConfiguration : IEntityTypeConfiguration<Asn>
    {
        public void Configure(EntityTypeBuilder<Asn> builder)
        {
            builder.ToTable("Asn");

            builder.HasKey(a => a.Code);
            //builder.Property(a => a.Id).UseIdentityColumn();
            builder.Property(a => a.PONumber).IsRequired().HasMaxLength(20);
            builder.Property(a => a.SupplierName).IsRequired().HasMaxLength(50);
            builder.Property(a => a.SupplierAddress).IsRequired().HasMaxLength(250);
            builder.Property(a => a.SupplierMail).HasMaxLength(100);
            builder.Property(a => a.SupplierPhone).HasMaxLength(20);
            builder.Property(a => a.Status).HasMaxLength(1).HasColumnType("char");
            builder.Property(a => a.ReleaseDate).IsRequired(false);
            builder.Property(a => a.ReceivingDate).IsRequired(false);
            builder.Property(a => a.DocName).HasMaxLength(100);
            builder.Property(a => a.Remark).HasMaxLength(100);
        }
    }
}
