using ProyectoFinal.Entities;

namespace ProyectoFinal;

public class ReturnController(AppDbContext context)
{
  private readonly AppDbContext _context = context;

  public void ReturnBook(Loan loan)
  {
    _context.Returns.Add(new Return() { Loan = loan, ReturnDate = DateTime.Now });
  }
}
