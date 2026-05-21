using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Models;

namespace WebApplication24_02.Data;

public partial class SampleDbContext : DbContext
{
    public SampleDbContext()
    {
    }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    // Tablas de tu proyecto
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ggory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B15157430");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD7BC6EAFE");
            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Products__Catego__5EBF139D");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            // Configuración del Cliente
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId);

            // CORRECCIÓN PARA LA TABLA INTERMEDIA (Muchos a Muchos)
            entity.HasMany(o => o.Products)
                  .WithMany(p => p.Orders)
                  .UsingEntity<Dictionary<string, object>>(
                      "OrderProduct",
                      j => j.HasOne<Product>().WithMany().HasForeignKey("ProductsProductId"),
                      j => j.HasOne<Order>().WithMany().HasForeignKey("OrdersOrderId"),
                      j =>
                      {
                          j.HasKey("OrdersOrderId", "ProductsProductId");
                          j.ToTable("OrderProduct");
                      });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}