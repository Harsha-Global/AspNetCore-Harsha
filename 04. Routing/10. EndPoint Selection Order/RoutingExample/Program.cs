using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
 options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

var app = builder.Build();

//Routing is automatically enabled.
//No need for app.UseRouting() anymore

// Eg: files/sample.txt
app.Map("files/{filename}.{extension}", async context =>
{
 string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
 string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

 await context.Response.WriteAsync($"In files - {fileName} - {extension}");
});


//Eg: employee/profile/john
app.Map("employee/profile/{EmployeeName:length(4, 7):alpha=scott}", async context =>
{
 string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
 await context.Response.WriteAsync($"In Employee profile - {employeeName}");
});


//Eg: products/details/1
app.Map("products/details/{id:int:range(1, 1000)?}", async context =>
{
 if (context.Request.RouteValues.ContainsKey("id"))
 {
  int id = Convert.ToInt32(context.Request.RouteValues["id"]);
  await context.Response.WriteAsync($"Product details - {id}");
 }
 else
 {
  await context.Response.WriteAsync($"Product details - id is not supplied");
 }
});


//Eg: daily-digest-report/{reportdate}
app.Map("daily-digest-report/{reportdate:datetime}", async (context) =>
{
 DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);

 await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
});


//Eg: cities/{cityid}
app.Map("cities/{cityid:guid}", async (context) =>
{
 Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
 await context.Response.WriteAsync($"City information - {cityId}");
});


//Eg: sales-report/2024/jan
app.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
{
 int year = Convert.ToInt32(context.Request.RouteValues["year"]);
 string? month = Convert.ToString(context.Request.RouteValues["month"]);

 await context.Response.WriteAsync($"sales report - {year} - {month}");
});


//Eg: sales-report/2024/jan
app.Map("sales-report/2024/jan", async (context) => 
{
 await context.Response.WriteAsync($"Sales report exclusively for 2024 - jan");
});


//Fallback for any other requests
app.MapFallback(async (context) =>
{
 await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});


app.Run();
