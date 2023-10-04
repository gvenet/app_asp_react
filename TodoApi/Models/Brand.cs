using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class Brand {
  public long Id { get; set; }
  public string? Label { get; set; }
  public string? Description { get; set; }
  public int? YearCreation { get; set; }
  
}
