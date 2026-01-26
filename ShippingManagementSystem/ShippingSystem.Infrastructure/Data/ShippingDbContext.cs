using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.Data
{
    public class ShippingDbContext:IdentityDbContext<ApplicationUser>
    {
        public ShippingDbContext():base()
        {
            
        }
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options):base(options)
        {
            
        }
        public DbSet<Branches> Branch { get; set; }
        public DbSet<Cities> City{ get; set; }
        public DbSet<Couriers> Courier { get; set; }
        public DbSet<Employees> Employee { get; set; }
        public DbSet<Governorates> Governorate { get; set; }
        public DbSet<Merchants> Merchant { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Orders> Order { get; set; }
        public DbSet<PaymentMethods> PaymentMethod { get; set; }
        public DbSet<ShippingTypes> ShippingType { get; set; }
        public DbSet<WeightSettings> WeightSettings { get; set; }
        public DbSet<Regions> Region { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
      

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasIndex(u => u.NormalizedEmail).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(u => u.NormalizedUserName).IsUnique();
            builder.Entity<Regions>().HasIndex(r=>r.Name).IsUnique();
            builder.Entity<Governorates>().HasIndex(g => g.Name).IsUnique();
            builder.Entity<Cities>().HasIndex(c => c.Name).IsUnique();
            builder.Entity<Branches>().HasIndex(b => b.Name).IsUnique();
            builder.Entity<WeightSettings>().HasIndex(w => w.CityId).IsUnique();

            base.OnModelCreating(builder);

        }
    }
}
