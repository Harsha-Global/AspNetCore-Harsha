using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
 public class Product
 {
  [Required(ErrorMessage = "Id can't be blank")]
  [Range(0, int.MaxValue, ErrorMessage = "Id should be between 0 to maximum value of int")]
  public int Id { get; set; }

  [Required(ErrorMessage = "Product Name can't be blank")]
  public string? ProductName { get; set; }

  public override string ToString()
  {
   return $"Product ID: {Id}, Product Name: {ProductName}";
  }
 }
}
