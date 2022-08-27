var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints =>
{
  //Eg: files/sample.txt
  endpoints.Map("files/{filename}.{extension}", async context =>
  {
    string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
    string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

    await context.Response.WriteAsync($"In files - {fileName} - {extension}");
  });

  //Eg: employee/profile/john
  endpoints.Map("employee/profile/{EmployeeName=harsha}", async context =>
  {
    string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
    await context.Response.WriteAsync($"In Employee profile - {employeeName}");
  });


 //Eg: products/details/
 endpoints.Map("products/details/{id?}", async context => {
  if (context.Request.RouteValues.ContainsKey("id"))
  {
   int id = Convert.ToInt32(context.Request.RouteValues["id"]);
   await context.Response.WriteAsync($"Products details - {id}");
  }
  else
  {
   await context.Response.WriteAsync($"Products details - id is not supplied");
  }
 });
});

app.Run(async context => {
  await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
