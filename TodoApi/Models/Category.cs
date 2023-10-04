using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class Category {
  public long Id { get; set; }
  public string? Label { get; set; }
  
  [JsonIgnore]
  public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
