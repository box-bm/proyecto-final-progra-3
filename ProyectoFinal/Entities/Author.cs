namespace ProyectoFinal.Entities;

public class Author
{
  public int AuthorId { get; set; }
  public required string Name { get; set; }

  public required List<Book> Books { get; set; }
}
