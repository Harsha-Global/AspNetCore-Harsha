using MinimalAPI.Models;
using MinimalAPI.RouteGroups;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var mapGroup = app.MapGroup("/products").ProductsAPI();


app.Run();
