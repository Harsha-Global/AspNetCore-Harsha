using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
 public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
 {
  public void OnResultExecuted(ResultExecutedContext context)
  {
   //TO DO: before logic here
  }

  public void OnResultExecuting(ResultExecutingContext context)
  {
   if (context.Filters.OfType<SkipFilter>().Any())
   {
    return;
   }
  }
 }
}
