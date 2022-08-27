using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
 public class PersonsListActionFilter : IActionFilter
 {
  private readonly ILogger<PersonsListActionFilter> _logger;

  public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
  {
   _logger = logger;
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
   //To do: add after logic here
   _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
   //To do: add before logic here
   _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");
  }
 }
}
