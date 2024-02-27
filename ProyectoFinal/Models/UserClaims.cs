namespace ProyectoFinal;

public class UserClaims(string name, string email, string? Username, List<string>? roles)
{
  public string? Username = Username;
  public string Email = email;
  public string Name = name;
  public List<string>? Roles = roles;
}
