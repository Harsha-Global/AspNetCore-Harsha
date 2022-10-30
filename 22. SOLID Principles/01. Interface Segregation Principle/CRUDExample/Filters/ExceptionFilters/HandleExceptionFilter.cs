using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ExceptionFilters
{
 public class HandleExceptionFilter : IExceptionFilter
 {
  private readonly ILogger<HandleExceptionFilter> _logger;
  private readonly IHostEnvironment _hostEnvironment;

  public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEvironment)
  {
   _logger = logger;
   _hostEnvironment = hostEvironment;
  }


  public void OnException(ExceptionContext context)
  {
   _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);

   if (_hostEnvironment.IsDevelopment())
    context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
  }
 }
}
