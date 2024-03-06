using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Entities;
using ProyectoFinal.Models;

namespace ProyectoFinal;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserManager<User> userManager) : ControllerBase
{

  private readonly UserManager<User> userManager = userManager;


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

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(int id)
  {
    var user = await userManager.FindByIdAsync(id.ToString());

    if (user == null)
    {
      return NotFound();
    }

    await userManager.DeleteAsync(user);
    return Ok();
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(int id, User user)
  {
    var registeredUser = await userManager.FindByIdAsync(id.ToString());

    if (registeredUser == null)
    {
      return NotFound();
    }

    registeredUser.Email = user.Email;
    registeredUser.Name = user.Name;
    registeredUser.Address = user.Address;
    registeredUser.UserName = user.UserName;

    await userManager.UpdateAsync(registeredUser);

    return Ok();
  }



}
