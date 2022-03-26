using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models.Configurations
{
    public class TransactionStatusConfiguration : IEntityTypeConfiguration<TransactionStatus>
    {
        public void Configure(EntityTypeBuilder<TransactionStatus> builder)
        {
            builder.ToTable("TransactionStatus");

            builder.HasKey(ts => ts.Id);
            builder.Property(ts => ts.Id).UseIdentityColumn();
            builder.Property(ts => ts.Code).HasMaxLength(1).HasColumnType("char");
            builder.Property(ts => ts.Status).IsRequired().HasMaxLength(50);
            builder.Property(ts => ts.Description).HasMaxLength(100);
            builder.Property(ts => ts.TransactionType).HasMaxLength(20);
        }
    }
}
