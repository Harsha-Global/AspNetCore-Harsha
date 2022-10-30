var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
  Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
  if (endPoint != null)
  {
    await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
  }
  await next(context);
});

//enable routing
app.UseRouting();

app.Use(async (context, next) =>
{
  Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
  if (endPoint != null)
  {
    await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
  }
  
  await next(context);
});

//creating endpoints
app.UseEndpoints(endpoints =>
{
  //add your endpoints here
  endpoints.MapGet("map1", async (context) => {
    await context.Response.WriteAsync("In Map 1");
  });

  endpoints.MapPost("map2", async (context) => {
    await context.Response.WriteAsync("In Map 2");
  });
});

app.Run(async context => {
  await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
