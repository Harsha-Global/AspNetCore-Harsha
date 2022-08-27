using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Add(new ServiceDescriptor(
  typeof(ICitiesService),
  typeof(CitiesService),
  ServiceLifetime.Singleton
));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
