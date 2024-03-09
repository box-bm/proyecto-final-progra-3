using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Entities;

namespace ProyectoFinal;

[ApiController]
[Route("api/[controller]")]
[Authorize("Admin")]
public class InventoryController(AppDbContext context) : ControllerBase
{
  private readonly AppDbContext _context = context;

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Inventory>>> GetAll()
  {
    if (_context.Inventory is null) return NotFound();

    return await _context.Inventory.ToListAsync();
  }

  [HttpPut("updateBookStock/{bookId}")]
  public async Task<IActionResult> UpdateBookStock(int bookId, int quantity)
  {
    var inventory = await AddStock(bookId, quantity);
    if (inventory.Balance < 0 || inventory.Available < 0) return BadRequest("Not enough stock");

    await _context.SaveChangesAsync();
    return Ok(inventory);
  }

  public void AddToInventory(Inventory item)
  {
    _context.Inventory.Add(item);
  }

  public async Task UpdateStock(int bookId, int quantity)
  {
    if (_context.Inventory is null) throw new Exception("Inventory not found");

    var inventory = await _context.Inventory.Where(i => i.BookId == bookId).FirstAsync();

    if (inventory is null) throw new Exception("Inventory not found");

    inventory.Borrowed -= quantity;
    inventory.Available += quantity;

    _context.Entry(inventory).State = EntityState.Modified;
  }

  public async Task<Inventory> AddStock(int bookId, int quantity)
  {
    if (_context.Inventory is null) throw new Exception("Inventory not found");

    var inventory = await _context.Inventory.Where(i => i.BookId == bookId).FirstAsync();
    if (inventory is null) throw new Exception("Inventory not found");

    inventory.Balance += quantity;
    inventory.Available += quantity;

    _context.Entry(inventory).State = EntityState.Modified;
    return inventory;
  }
}
