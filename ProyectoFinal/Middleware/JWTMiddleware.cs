namespace ProyectoFinal.Midleware;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class JwtMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task Invoke(HttpContext context)
  {
    var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

    if (token is not null)
    {
      context.Items["Token"] = token;
      var email = context.User.Claims.Where((claim) => claim.Type == "Email").FirstOrDefault()?.Value;

      if (email is not null)
      {
        context.Items["Email"] = email;
      }
    }

    await _next(context);
  }
}

public static class JwtMiddlewareExtensions
{
  public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<JwtMiddleware>();
  }
}
