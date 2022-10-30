using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
  public class ValidationHelper
  {
    internal static void ModelValidation(object obj)
    {
      //Model validations
      ValidationContext validationContext = new ValidationContext(obj);
      List<ValidationResult> validationResults = new List<ValidationResult>();

      bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
      if (!isValid)
      {
        throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
      }
    }
  }
}
