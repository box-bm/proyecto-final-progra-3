using Microsoft.AspNetCore.Identity;
using ProyectoFinal.Entities;

namespace ProyectoFinal.Utils;


public class SeedConfig(WebApplication app)
{
  public async Task SeedData()
  {
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    if (!userManager.Users.Any())
    {
      logger.LogInformation("Creando usuario de prueba");

      var newUser = new User
      {
        Email = "test@demo.com",
        UserName = "test.demo",
        Name = "Test",
        Address = "Test address",
      };

      await userManager.CreateAsync(newUser, "P@ss.W0rd");
      await roleManager.CreateAsync(new IdentityRole
      {
        Name = "Admin"
      });
      await roleManager.CreateAsync(new IdentityRole
      {
        Name = "Customer"
      });

      await userManager.AddToRoleAsync(newUser, "Admin");
    }
  }
}
