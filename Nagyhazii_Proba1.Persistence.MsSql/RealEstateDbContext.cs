using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nagyhazii_Proba1.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Nagyhazii_Proba1.Persistence.MsSql
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options) { }

        // DbSet-ek az egyes entitásokhoz
        public DbSet<Property> Properties { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<PropertyOwner> PropertyOwners { get; set; } // Kapcsolati entitás

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Beállítjuk a decimal mezők pontos típusát
            modelBuilder.Entity<Contract>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Customer>()
                .Property(c => c.MinPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Customer>()
                .Property(c => c.MaxPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Property>()
                .Property(p => p.SellingPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Property>()
                .Property(p => p.RentPrice)
                .HasColumnType("decimal(18, 2)");

            // Property - Customer (Many-to-Many) kapcsolat konfigurálása PropertyOwner entitással
            modelBuilder.Entity<PropertyOwner>()
                .HasKey(po => new { po.PropertyId, po.CustomerId });

            modelBuilder.Entity<PropertyOwner>()
                .HasOne(po => po.Property)
                .WithMany(p => p.PropertyOwners)
                .HasForeignKey(po => po.PropertyId);

            modelBuilder.Entity<PropertyOwner>()
                .HasOne(po => po.Customer)
                .WithMany(c => c.OwnedProperties)
                .HasForeignKey(po => po.CustomerId);

            // Contract - Property kapcsolat (One-to-One)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Property)
                .WithMany()
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contract - Seller kapcsolat (One-to-Many)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Seller)
                .WithMany()
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contract - Buyer kapcsolat (One-to-Many)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Buyer)
                .WithMany()
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
