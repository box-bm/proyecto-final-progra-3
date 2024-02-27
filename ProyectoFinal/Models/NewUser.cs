namespace ProyectoFinal.Models;

public class NewUser
{
  public required string Email { get; set; }
  public required string Password { get; set; }
  public required string Name { get; set; }
  public string Address { get; set; } = "";
  public required string UserName { get; set; }
}
