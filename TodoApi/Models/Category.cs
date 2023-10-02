namespace TodoApi.Models;

public class Category {
  public long Id { get; set; }
  public string? Label { get; set; }
  
  public ICollection<Product> Products { get; set; } = new List<Product>();

}
