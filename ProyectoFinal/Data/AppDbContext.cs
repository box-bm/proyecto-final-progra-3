using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  // Here all entities
  // public DbSet<Model> ModelName { set; get; }
  public DbSet<Article> Articles { set; get; }
}
