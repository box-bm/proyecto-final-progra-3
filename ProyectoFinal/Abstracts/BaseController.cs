using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal;

public abstract class BaseController<T>(AppDbContext context) : ControllerBase
{

  public readonly AppDbContext _context = context;

  public abstract Task<ActionResult<IEnumerable<T>>> GetAll();

  public abstract Task<ActionResult<T>> GetByID(int id);

  public abstract Task<ActionResult<T>> Create(T item);

  public abstract Task<IActionResult> Update(int id, T item);

  public abstract Task<IActionResult> Delete(int id);
}
