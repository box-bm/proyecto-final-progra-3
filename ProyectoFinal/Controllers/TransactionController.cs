using ProyectoFinal.Entities;

namespace ProyectoFinal;

public class TransactionController(AppDbContext context)
{
  private readonly AppDbContext _context = context;

  public void NewLoan(Book book, User user, string? comment = null)
  {
    _context.Transactions.Add(new Transaction() { Book = book, User = user, TransactionDate = DateTime.Now, TransactionType = TransactionType.Loan, Comment = comment });
  }
}
