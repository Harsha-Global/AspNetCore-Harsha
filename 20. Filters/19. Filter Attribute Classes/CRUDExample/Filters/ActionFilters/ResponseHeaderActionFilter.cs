using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
 public class ResponseHeaderActionFilter : ActionFilterAttribute
 {
  private readonly string _key;
  private readonly string _value;

  public ResponseHeaderActionFilter(string key, string value, int order)
  {
   _key = key;
   _value = value;
   Order = order;
  }

  public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
   await next(); //calls the subsequent filter or action method

   context.HttpContext.Response.Headers[_key] = _value;
  }
 }
}
