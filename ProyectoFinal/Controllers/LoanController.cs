using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Entities;

namespace ProyectoFinal;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanController(AppDbContext context, UserManager<User> userManager) : BaseController<Loan>(context)
{
  private readonly InventoryController inventoryController = new(context);
  private readonly TransactionController transactionController = new(context);
  private readonly ReturnController returnController = new(context);
  private readonly UserManager<User> _userManager = userManager;

  [HttpGet]
  public override async Task<ActionResult<IEnumerable<Loan>>> GetAll()
  {
    if (_context.Loans is null) return NotFound();

    return await _context.Loans.ToListAsync();
  }

  [HttpGet("{id}")]
  public override async Task<ActionResult<Loan>> GetByID(int id)
  {
    if (_context.Loans is null) return NotFound();

    var loan = await _context.Loans.FindAsync(id);
    if (loan is null) return NotFound();

    return loan;
  }

  [HttpGet("borrowed")]
  public async Task<ActionResult<IEnumerable<Loan>>> GetBorrowedBooksByUser()
  {
    if (HttpContext.Items["Email"] is not string email) return Unauthorized();
    if (_context.Loans is null) return NoContent();

    var user = await _userManager.FindByEmailAsync(email);
    var books = await _context.Loans.Where((loan) => loan.User == user).ToListAsync();
    return books;
  }

  [HttpPost]
  public override async Task<ActionResult<Loan>> Create(Loan item)
  {
    if (HttpContext.Items["Email"] is not string email) return Unauthorized();
    var user = await _userManager.FindByEmailAsync(email);

    if (user is null) return NotFound();
    item.User = user;
    var newLoan = _context.Loans.Add(item);

    transactionController.NewLoan(newLoan.Entity.Book, user);
    await inventoryController.UpdateStock(newLoan.Entity.Book.BookId, -1);

    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetByID), new { id = item.LoanId }, item);
  }

  [HttpPost("asign")]
  public async Task<ActionResult<Loan>> AsignLoan(Loan item, string userEmail)
  {
    if (HttpContext.Items["Email"] is not string email) return Unauthorized();
    var user = await _userManager.FindByEmailAsync(email);
    if (user is null) return Unauthorized();

    var asigneeUser = await _userManager.FindByEmailAsync(userEmail);

    if (asigneeUser is null) return NotFound();
    item.User = asigneeUser;
    var newLoan = _context.Loans.Add(item);

    transactionController.NewLoan(newLoan.Entity.Book, user, asigneeUser);
    await inventoryController.UpdateStock(newLoan.Entity.Book.BookId, -1);

    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetByID), new { id = item.LoanId }, item);
  }

  [HttpDelete("{id}")]
  public override async Task<IActionResult> Delete(int id)
  {
    if (_context.Loans is null) return NotFound();
    var loan = _context.Loans.Find(id);

    if (loan is null) return NotFound();
    _context.Loans.Remove(loan);
    await _context.SaveChangesAsync();
    return Ok();
  }

  [HttpPut("{id}")]
  public override Task<IActionResult> Update(int id, Loan item)
  {
    throw new NotImplementedException();
  }

  [HttpPut("return/{id}")]
  public async Task<IActionResult> ReturnLoan(int id)
  {
    var loan = await _context.Loans.FindAsync(id);
    if (loan is null) return NotFound();
    loan.IsReturned = true;

    returnController.ReturnBook(loan);
    transactionController.NewReturn(loan.Book, loan.User);
    await inventoryController.UpdateStock(loan.Book.BookId, 1);

    await _context.SaveChangesAsync();
    return Ok();
  }
}
