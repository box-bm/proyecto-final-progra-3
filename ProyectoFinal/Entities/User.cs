using Microsoft.AspNetCore.Identity;

namespace ProyectoFinal.Entities;

public class User : IdentityUser
{
  public required string Name { get; set; }
  public required string Address { get; set; }
}
