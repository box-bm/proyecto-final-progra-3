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
  private string GetAudience() => _config["Jwt:Audience"] ?? throw new Exception("Jwt:Audience not found in appsettings.json");

  public string CreateToken(List<Claim> userClaims)
  {

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetKey()));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var strToken = new JwtSecurityToken(GetIssuer(), GetAudience(), userClaims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
    var token = new JwtSecurityTokenHandler().WriteToken(strToken);

    return token;
  }
}
