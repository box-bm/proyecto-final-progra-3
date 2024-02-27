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
  public async Task<ActionResult> Create([FromBody] INewUser user)
  {
    try
    {
      var newUser = new User
      {
        UserName = user.UserName,
        Email = user.Email,
        Name = user.Name,
        Address = user.Address
      };

      var result = await userManager.CreateAsync(newUser, user.Password);

      return Created();
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

  [HttpPost]
  [Route("login")]
  public async Task<ActionResult> Login([FromBody] IUserCredentials userCredentials)
  {
    try
    {
      var user = await userManager.FindByEmailAsync(userCredentials.Email);

      if (user == null) return NotFound();

      var isValid = await userManager.CheckPasswordAsync(user, userCredentials.Password);
      if (!isValid) return Unauthorized();

      var roles = await userManager.GetRolesAsync(user);

      var token = new JWTConfig(_config).CreateToken(new UserClaims(user.Name, user.Email!, user.UserName, [.. roles]));
      return Ok(token);
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

}
