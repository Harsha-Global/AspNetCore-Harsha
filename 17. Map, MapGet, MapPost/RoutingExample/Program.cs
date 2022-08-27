var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();

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
