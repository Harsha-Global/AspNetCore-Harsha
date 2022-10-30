using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
 //Eg: sales-report/2020/apr
 public class MonthsCustomConstraint : IRouteConstraint
 {
  public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
  {
   //check whether the value exists
   if (!values.ContainsKey(routeKey)) //month
   {
    return false; //not a match
   }

   Regex regex = new Regex("^(apr|jul|oct|jan)$");
   string? monthValue = Convert.ToString(values[routeKey]);

   if (regex.IsMatch(monthValue))
   {
    return true; //it's a match
   }
   return false; //not a match
  }
 }
}
