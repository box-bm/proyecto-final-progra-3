using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProyectoFinal.Utils;

public class JWTConfig(IConfiguration config)
{
  private IConfiguration _config = config;

  private string GetKey() => _config["Jwt:Key"] ?? throw new Exception("Jwt:Key not found in appsettings.json");
  private string GetIssuer() => _config["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer not found in appsettings.json");

  public string CreateToken(UserClaims userClaims)
  {

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetKey()));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claimProperties = typeof(UserClaims).GetProperties();
    var claims = new List<Claim>();

    foreach (var porperty in claimProperties)
    {
      var value = porperty.GetValue(userClaims);
      claims.Add(new Claim(porperty.Name, value?.ToString() ?? ""));
    }
    var strToken = new JwtSecurityToken(GetIssuer(), GetIssuer(), claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(strToken);
  }
}
