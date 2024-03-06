namespace ProyectoFinal.Entities;

public class Transaction
{
  public int TransactionID { get; set; }
  public required TransactionType TransactionType { get; set; }
  public required DateTime TransactionDate { get; set; } = DateTime.Now;
  public required Book Book { get; set; }
  public required User User { get; set; }
  public string? Comment { get; set; } = null;
}
