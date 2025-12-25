using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Supply an object of WeatherApiOptions (with 'weatherapi' section) as a service
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherapi"));

//Load MyOwnConfig.json
builder.Configuration.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.Run();
