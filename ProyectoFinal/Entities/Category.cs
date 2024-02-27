namespace ProyectoFinal.Entities;

public class Category
{
  public int CategoryId { get; set; }
  public required string Name { get; set; }

  public required List<Book> Books { get; set; }
}
