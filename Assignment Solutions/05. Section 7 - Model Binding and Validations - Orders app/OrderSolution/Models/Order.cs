using OrderSolution.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace OrderSolution.Models
{
 public class Order
 {
  [Display(Name = "Order Number")]
  public int? OrderNo { get; set; }

  [Required(ErrorMessage = "{0} can't be blank")]
  [Display(Name = "Order Date")]
  [MinimumDateValidator("2000-01-01", ErrorMessage = "Order Date should be greater than or equal to 2000")] //custom validator
  public DateTime OrderDate { get; set; }

  [Required(ErrorMessage = "{0} can't be blank")]
  [Display(Name = "Invoice Price")]
  [InvoicePriceValidator] //custom validator
  [Range(1, double.MaxValue, ErrorMessage = "{0} should be between a valid number")]
  public double InvoicePrice { get; set; }

  [ProductsListValidator] //custom validator
  public List<Product> Products { get; set; } = new List<Product>();
 }
}

