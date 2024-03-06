namespace ProyectoFinal.Entities;

public class Book
{
  public int BookId { get; set; }
  public required string Title { get; set; }
  public required string CodeBar { get; set; }
  public required int Year { get; set; }
  public required string Cover { get; set; }
  public required string Prologue { get; set; }

  public required Author Author { get; set; }
  public required Category Category { get; set; }
  public required Inventory Inventory { get; set; }
  public required List<Loan> Loans { get; set; }
  public required List<Transaction> Transactions { get; set; }
}
