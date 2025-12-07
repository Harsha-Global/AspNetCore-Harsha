var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Routing is automatically enabled.
//No need for app.UseRouting() anymore

//creating endpoints
app.MapGet("map1", async (context) => {
 await context.Response.WriteAsync("In Map 1");
});

app.MapPost("map2", async (context) => {
 await context.Response.WriteAsync("In Map 2");
});

//Fallback for any other requests
app.MapFallback(async context => {
 await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
