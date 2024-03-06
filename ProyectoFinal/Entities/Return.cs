namespace ProyectoFinal.Entities;

public class Return
{
  public int ReturnId { get; set; }
  public required Loan Loan { get; set; }
  public DateTime ReturnDate { get; set; } = DateTime.Now;
}
