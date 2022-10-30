using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
 public class PersonsListResultFilter : IAsyncResultFilter
 {
  private readonly ILogger<PersonsListResultFilter> _logger;

  public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
  {
   _logger = logger;
  }

  public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
  {
   //TO DO: before logic
   _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

   await next(); //call the subsequent filter [or] IActionResult

   //TO DO: after logic
   _logger.LogInformation("{FilterName}.{MethodName} - after", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

   context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
  }
 }
}
