using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal;


[Route("API/[controller]")]
[ApiController]
public class TemplateController(AppDbContext context) : BaseController<Article>(context)
{
  [HttpGet]
  public async override Task<ActionResult<IEnumerable<Article>>> GetAll()
  {
    return await _context.Articles.ToListAsync();
  }

  [HttpGet("{id}")]
  public async override Task<ActionResult<Article>> GetByID(int id)
  {
    var article = await _context.Articles.FindAsync(id);

    if (article == null)
    {
      return NotFound();
    }

    return article;
  }

  [HttpPost]
  public async override Task<ActionResult<Article>> Create(Article article)
  {
    article.CreatedAt = DateTime.Now;
    _context.Articles.Add(article);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetArticle", new { id = article.Id }, article);
  }

  [HttpPut("{id}")]
  public async override Task<IActionResult> Update(int id, Article article)
  {
    if (id != article.Id)
    {
      return BadRequest();
    }

    _context.Entry(article).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!ArticleExists(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  [HttpDelete("{id}")]
  public async override Task<IActionResult> Delete(int id)
  {
    var article = await _context.Articles.FindAsync(id);

    if (article == null)
    {
      return NotFound();
    }

    _context.Articles.Remove(article);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool ArticleExists(int id)
  {
    return _context.Articles.Any(e => e.Id == id);
  }
}