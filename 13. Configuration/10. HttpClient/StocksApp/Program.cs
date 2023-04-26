using StocksApp;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);

//Services

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<FinnhubService>();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection(nameof(TradingOptions))); //add IOptions<TradingOptions> as a service
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
