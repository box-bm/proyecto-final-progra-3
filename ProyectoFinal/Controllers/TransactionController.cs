using ProyectoFinal.Entities;

namespace ProyectoFinal;

public class TransactionController(AppDbContext context)
{
  private readonly AppDbContext _context = context;

  public void NewLoan(Book book, User user, string? comment = null)
  {
    _context.Transactions.Add(new Transaction() { Book = book, User = user, TransactionDate = DateTime.Now, TransactionType = TransactionType.Loan, Comment = comment });
  }

  public void NewLoan(Book book, User user, User asignedUser, string? comment = null)
  {
    _context.Transactions.Add(new Transaction()
    {
      Book = book,
      User = user,
      TransactionDate = DateTime.Now,
      TransactionType = TransactionType.Loan,
      Comment = "Se le ha prestado el libro: " + book.Title + " al usuario: " + asignedUser.Name + " Comentario: " + comment,
    });
  }

  public void NewReturn(Book book, User user)
  {
    _context.Transactions.Add(new Transaction() { Book = book, User = user, TransactionDate = DateTime.Now, TransactionType = TransactionType.Return });
  }

  public void NewBookAddition(Book book, User user, int count)
  {
    _context.Transactions.Add(new Transaction() { Book = book, User = user, TransactionDate = DateTime.Now, TransactionType = TransactionType.Adition, Comment = "Se han agregado " + count + " unidades del libro: " + book.Title });
  }

  public void NewBookDeletion(Book book, User user, int count)
  {
    _context.Transactions.Add(new Transaction() { Book = book, User = user, TransactionDate = DateTime.Now, TransactionType = TransactionType.Deletion, Comment = "Se han eliminado " + count + " unidades del libro: " + book.Title });
  }
}
