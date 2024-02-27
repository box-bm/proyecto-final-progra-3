using System.Security.Claims;
using Microsoft.VisualBasic;

namespace ProyectoFinal;

public class UserClaims(string name, string email, string? Username, List<string>? roles)
{
  public string? Username = Username;
  public string Email = email;
  public string Name = name;
  public List<string>? Roles = roles;

  public List<Claim> GetClaims()
  {
    var claimProperties = typeof(UserClaims).GetProperties();

    var claims = new List<Claim>();

    foreach (var property in claimProperties)
    {
      var value = property.GetValue(this);
      claims.Add(new Claim(property.Name, value?.ToString() ?? ""));
    }

    return claims;
  }
}
