var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//middlware 1
app.Use(async ( context, next) => {
    await context.Response.WriteAsync("Hello");
    await next(context);
});

//middleware 2
app.Use(async (context, next) => {
    await context.Response.WriteAsync("Hello again");
    await next(context);
});

//middleware 3
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Hello again");
});


app.Run();
