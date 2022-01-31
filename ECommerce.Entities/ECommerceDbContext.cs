using ECommerce.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECommerce.Entities
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Category>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Product>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Role>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<ProductImage>().Property(x => x.IsActive).HasDefaultValue(true);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
