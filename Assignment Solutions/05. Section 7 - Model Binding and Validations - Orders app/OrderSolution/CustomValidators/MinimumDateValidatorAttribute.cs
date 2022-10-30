using OrderSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderSolution.CustomValidators
{
 public class MinimumDateValidatorAttribute : ValidationAttribute
 {
  public string DefaultErrorMessage { get; set; } = "Order date should be greater than or equal to {0}";
  public DateTime MinimumDate { get; set; }

  public MinimumDateValidatorAttribute(string minimumDateString)
  {
   //According to CS0181 rule, we can't use DateTime data type as one of the parameter types in an attribute class
   MinimumDate = Convert.ToDateTime(minimumDateString);
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
   //check if the value of "OrderDate" property is not null
   if (value != null)
   {
    //get the value of "OrderDate" property
    DateTime orderDate = (DateTime)value;

    //if the value of "OrderDate" property is greater than minimumDate
    if (orderDate > MinimumDate)
    {
     //return validation error
     return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumDate.ToString("yyyy-MM-dd")), new string[] { nameof(validationContext.MemberName) });
    }

    //No validation error
    return ValidationResult.Success;
   }
   return null;
  }
 }
}

