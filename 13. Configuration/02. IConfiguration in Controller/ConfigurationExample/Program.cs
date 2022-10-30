var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {

  endpoints.Map("/config", async context =>
  {
    await context.Response.WriteAsync(app.Configuration["mykEY"] + "\n");

    await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");

    await context.Response.WriteAsync(app.Configuration.GetValue<int>("x", 10) + "\n");

  });

});

app.MapControllers();

app.Run();
