using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Models;
using WMSApplication.Models.Configurations;

namespace WMSApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionStatusConfiguration());
            modelBuilder.ApplyConfiguration(new AsnConfiguration());
            modelBuilder.ApplyConfiguration(new AsnDetailConfiguration());

            modelBuilder.ApplyConfiguration(new AuthUserConfiguration());
            modelBuilder.ApplyConfiguration(new AuthRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AuthUserRoleConfiguration());
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Asn> Asn { get; set; }
        public DbSet<AsnDetail> AsnDetail { get; set; }

        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<AuthRole> AuthRoles { get; set; }
        public DbSet<AuthUserRole> AuthUserRole { get; set; }
    }
}
