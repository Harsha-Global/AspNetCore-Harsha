using OrderSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderSolution.CustomValidators
{
 public class ProductsListValidatorAttribute : ValidationAttribute
 {
  public string DefaultErrorMessage { get; set; } = "Order should have at least one product";

  public ProductsListValidatorAttribute()
  {
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
   //check if the value of "Products" property is not null
   if (value != null)
   {
    List<Product> products = (List<Product>)value;

    //if no products
    if (products.Count == 0)
    {
     //return validation error
     return new ValidationResult(DefaultErrorMessage, new string[] { nameof(validationContext.MemberName) });
    }

    //No validation error
    return ValidationResult.Success;
   }
   return null;
  }
 }
}

