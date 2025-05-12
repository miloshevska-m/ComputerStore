using ComputerStore.data.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.data
{
    public class ComputerStoreContext : DbContext
    {
        public ComputerStoreContext(DbContextOptions<ComputerStoreContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
            .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
            .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Product>()
            .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
        }

    }
}