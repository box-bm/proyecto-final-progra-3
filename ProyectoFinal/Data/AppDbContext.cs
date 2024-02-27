using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Local dependencies
using ProyectoFinal.Entities;

namespace ProyectoFinal;


public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
  // Here all entities
  // public DbSet<Model> ModelName { set; get; }

  public DbSet<Author> Authors { set; get; }

  public DbSet<Book> Books { set; get; }

  public DbSet<Category> Categories { set; get; }
}
