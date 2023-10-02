using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class ApiContext : DbContext {
  public ApiContext(DbContextOptions<ApiContext> options)
      : base(options) { }

  public DbSet<Todo> Todos { get; set; } = null!;
  public DbSet<Login> Logins { get; set; } = null!;
  public DbSet<Product> Products { get; set; } = null!;
  public DbSet<Category> Categories { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    // Configuration de la relation one-to-many entre Category et Product
    modelBuilder.Entity<Category>()
        .HasMany(category => category.Products)
        .WithOne(product => product.Category)
        .HasForeignKey(product => product.CategoryId)
        .IsRequired();
  }
}
