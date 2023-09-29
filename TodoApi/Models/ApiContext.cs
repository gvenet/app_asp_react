using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class ApiContext : DbContext {
  public ApiContext(DbContextOptions<ApiContext> options)
      : base(options) { }

  public DbSet<Todo> Todos { get; set; } = null!;
  public DbSet<Login> Logins { get; set; } = null!;
  public DbSet<Product> Products { get; set; } = null!;
}
