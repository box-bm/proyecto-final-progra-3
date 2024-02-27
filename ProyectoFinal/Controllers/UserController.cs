using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Entities;
using ProyectoFinal.Models;
using ProyectoFinal.Utils;

namespace ProyectoFinal;

[Route("api/[controller]")]
[ApiController]
public class UserController(AppDbContext context, UserManager<User> userManager, IConfiguration config) : ControllerBase
{
  private readonly AppDbContext _context = context;
  private readonly UserManager<User> userManager = userManager;
  private IConfiguration _config = config;

  [HttpPost]
  [Route("register")]
  public async Task<ActionResult> Create(NewUser user)
  {
    try
    {
      var newUser = new User
      {
        UserName = user.UserName,
        Email = user.Email,
        Name = user.Name,
        Address = user.Address,
      };

      var result = await userManager.CreateAsync(newUser, user.Password);

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(newUser, "Customer");
        return Created();
      }
      else
        return StatusCode(StatusCodes.Status412PreconditionFailed, result.Errors);
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

  [HttpPost]
  [Route("login")]
  public async Task<ActionResult> Login(UserCredentials userCredentials)
  {
    try
    {
      var user = await userManager.FindByEmailAsync(userCredentials.Email);

      if (user == null) return NotFound();

      var isValid = await userManager.CheckPasswordAsync(user, userCredentials.Password);
      if (!isValid) return Unauthorized();

      var roles = await userManager.GetRolesAsync(user);
      await userManager.AddLoginAsync(user, new UserLoginInfo("Local", userCredentials.Email, "Local"));

      var claims = new UserClaims(user.Name, user.Email!, user.UserName, [.. roles]).GetClaims();
      var token = new JWTConfig(_config).CreateToken(claims);

      await userManager.SetAuthenticationTokenAsync(user, "Local", "Token", token);
      await userManager.AddClaimsAsync(user, claims);

      return Ok(new { token });
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

}
