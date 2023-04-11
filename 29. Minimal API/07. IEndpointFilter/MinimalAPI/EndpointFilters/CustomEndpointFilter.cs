namespace MinimalAPI.EndpointFilters
{
 public class CustomEndpointFilter : IEndpointFilter
 {
  private readonly ILogger<CustomEndpointFilter> _logger;

  public CustomEndpointFilter(ILogger<CustomEndpointFilter> logger)
  {
   _logger = logger;
  }

  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
   //Before logic
   _logger.LogInformation("Endpoint filter - before logic");

   var result = await next(context); //It invokes the subsequent filter or endpoint's request delegate


   //After logic
   _logger.LogInformation("Endpoint filter - after logic");

   return result;
  }
 }
}
