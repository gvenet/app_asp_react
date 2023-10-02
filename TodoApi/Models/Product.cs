using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class Product {
  public long Id { get; set; }
  public string? Label { get; set; }
  public float Price { get; set; }
  public string? Description { get; set; }
  public string? Image_Url { get; set; }
  public float Version { get; set; }

  public long CategoryId { get; set; }
  
  [JsonIgnore]
  public Category Category { get; set; } = null!;


}
