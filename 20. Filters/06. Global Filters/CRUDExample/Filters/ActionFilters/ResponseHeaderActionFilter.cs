using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
 public class ResponseHeaderActionFilter : IActionFilter
 {
  private readonly ILogger<ResponseHeaderActionFilter> _logger;
  private readonly string Key;
  private readonly string Value;

  public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
  {
   _logger = logger;
   Key = key;
   Value = value;
  }


  //before
  public void OnActionExecuting(ActionExecutingContext context)
  {
   _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));
  }

  //after
  public void OnActionExecuted(ActionExecutedContext context)
  {
   _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));

   context.HttpContext.Response.Headers[Key] = Value;
  }
 }
}
