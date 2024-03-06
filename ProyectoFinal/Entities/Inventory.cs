namespace ProyectoFinal.Entities;

public class Inventory
{
  public required int InventoryId { get; set; }

  public required int Available { get; set; }
  public required int Borrowed { get; set; }
  public required int Balance { get; set; }

  // Relations
  public required int BookId { get; set; }
  public required Book Book { get; set; }
}
