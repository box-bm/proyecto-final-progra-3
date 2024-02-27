namespace ProyectoFinal.Models;

public interface INewUser
{
  string Email { get; }
  string Password { get; }
  string Name { get; }
  string Address { get; }
  string UserName { get; }
}
