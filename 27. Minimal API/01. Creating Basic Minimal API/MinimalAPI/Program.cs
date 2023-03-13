var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//GET /
app.MapGet("/", async (HttpContext context) => {
 await context.Response.WriteAsync("GET - Hello World");
});

//POST /
app.MapPost("/", async (HttpContext context) => {
 await context.Response.WriteAsync("POST - Hello World");
});


app.Run();
