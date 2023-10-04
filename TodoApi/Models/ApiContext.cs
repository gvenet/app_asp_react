using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class ApiContext : DbContext {
  public ApiContext(DbContextOptions<ApiContext> options)
      : base(options) { }

  public DbSet<Todo> Todos { get; set; } = null!;
  public DbSet<Login> Logins { get; set; } = null!;
  public DbSet<Product> Products { get; set; } = null!;
  public DbSet<Category> Categories { get; set; } = null!;
  public DbSet<ProductCategory> ProductsCategories { get; set; } = null!;
  public DbSet<Brand> Brands { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ProductCategory>()
        .HasKey(pc => new { pc.ProductId, pc.CategoryId });

    modelBuilder.Entity<ProductCategory>()
        .HasOne(pc => pc.Product)
        .WithMany(p => p.ProductCategories)
        .HasForeignKey(pc => pc.ProductId);

    modelBuilder.Entity<ProductCategory>()
        .HasOne(pc => pc.Category)
        .WithMany(c => c.ProductCategories)
        .HasForeignKey(pc => pc.CategoryId);
}
}
