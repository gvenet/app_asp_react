using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class ProductViewModel {
  public string? Label { get; set; }
  public float Price { get; set; }
  public string? Description { get; set; }
  public string? Image_Url { get; set; }
  public float Version { get; set; }
  public string? BrandLabel { get; set; }
  public List<string> Categories { get; set; }
}
