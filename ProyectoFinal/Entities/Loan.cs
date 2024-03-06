using ProyectoFinal.Entities;

namespace ProyectoFinal;

/// <summary>
/// Loans
/// 
/// Contains information about books loaned.
/// </summary>
public class Loan
{
  public required int LoanId { get; set; }
  public required User User { get; set; }
  public required Book Book { get; set; }
  public required DateTime LoanDate { get; set; }
  public DateTime? DueDate { get; set; } = null;
  public bool IsReturned { get; set; } = false;
}
