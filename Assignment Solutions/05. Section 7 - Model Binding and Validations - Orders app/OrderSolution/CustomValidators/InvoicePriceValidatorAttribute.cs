using OrderSolution.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OrderSolution.CustomValidators
{
 public class InvoicePriceValidatorAttribute : ValidationAttribute
 {
  public string DefaultErrorMessage { get; set; } = "Invoice Price should be equal to the total cost of all products (i.e. {0}) in the order.";

  public InvoicePriceValidatorAttribute()
  {
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
   //check if the value of "InvoicePrice" property is not null
   if (value != null)
   {
    //get "Products" property reference, using reflection
    PropertyInfo? OtherProperty = validationContext.ObjectType.GetProperty(nameof(Order.Products));
    if (OtherProperty != null)
    {
     //get value of "Products" property of the current object of "Order" class
     List<Product> products = (List<Product>)OtherProperty.GetValue(validationContext.ObjectInstance)!; //"!" operator specifies that the value returned by GetValue() will not be null

     //calculate total price
     double totalPrice = 0;
     foreach (Product product in products)
     {
      totalPrice += product.Price * product.Quantity;
     }

     //value of "InvoicePrice" property
     double actualPrice = (double)value;

     if (totalPrice > 0)
     {
      //if the value of "InvoicePrice" property is not equal to the total cost of all products in the order
      if (totalPrice != actualPrice)
      {
       //return model error
       return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, totalPrice), new string[] { nameof(validationContext.MemberName) });
      }
     }
     else
     {
      //return model error is no products found
      return new ValidationResult("No products found to validate invoice price", new string[] { nameof(validationContext.MemberName) });
     }

     //No validation error
     return ValidationResult.Success;
    }
    return null;
   }
   else
    return null;
  }
 }
}

