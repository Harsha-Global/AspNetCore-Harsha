using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
 public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
 {
  public bool IsReusable => false;
  private string? Key { get; set; }
  private string? Value { get; set; }
  private int Order { get; set; }

  public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
  {
   Key = key;
   Value = value;
   Order = order;
  }

  //Controller -> FilterFactory -> Filter
  public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
  {
   var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
   filter.Key = Key;
   filter.Value = Value;
   filter.Order = Order;
   //return filter object
   return filter;
  }
 }

 public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
 {
  public string Key { get; set; }
  public string Value { get; set; }
  public int Order { get; set; }
  private readonly ILogger<ResponseHeaderActionFilter> _logger;

  public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
  {
   _logger = logger;
  }

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
   _logger.LogInformation("Before logic - ResponseHeaderActionFilter");

   await next(); //calls the subsequent filter or action method

   _logger.LogInformation("Before logic - ResponseHeaderActionFilter");

   context.HttpContext.Response.Headers[Key] = Value;
  }
 }
}
