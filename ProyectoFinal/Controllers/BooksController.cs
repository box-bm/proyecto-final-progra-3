using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Entities;

namespace ProyectoFinal;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BooksController(AppDbContext context) : BaseController<Book>(context)
{
  [HttpPost]
  public override async Task<ActionResult<Book>> Create(Book item)
  {
    _context.Books.Add(item);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetByID), new { id = item.BookId }, item);
  }

  [HttpDelete("{id}")]
  public override async Task<IActionResult> Delete(int id)
  {
    if (_context.Books is null) return NotFound();

    var book = await _context.Books.FindAsync(id);
    if (book is null) return NotFound();

    _context.Books.Remove(book);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpGet]
  public override async Task<ActionResult<IEnumerable<Book>>> GetAll()
  {
    if (_context.Books is null)
    {
      return NotFound();
    }

    return await _context.Books.ToListAsync();
  }

  [HttpGet("{id}")]
  public override async Task<ActionResult<Book>> GetByID(int id)
  {
    if (_context.Books is null)
    {
      return NotFound();
    }

    var books = await _context.Books.FindAsync(id);
    if (books is null)
    {
      return NotFound();
    }

    return books;
  }

  [HttpPut("{id}")]
  public override async Task<IActionResult> Update(int id, Book item)
  {
    if (id != item.BookId) return BadRequest();

    try
    {
      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }
    catch (Exception)
    {
      if (_context.Books.Any(e => e.BookId == id)) return NotFound();
      else throw;
    }

    return NoContent();
  }
}
