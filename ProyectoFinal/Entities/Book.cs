namespace ProyectoFinal.Entities;

public class Book
{
  public int BookId { get; set; }
  public required string Title { get; set; }
  public required string CodeBar { get; set; }

  public required Author Author { get; set; }
  public required Category Category { get; set; }
}
