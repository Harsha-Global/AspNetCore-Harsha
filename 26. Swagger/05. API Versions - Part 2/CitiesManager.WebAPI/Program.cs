using CitiesManager.WebAPI.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
 options.Filters.Add(new ProducesAttribute("application/json"));
 options.Filters.Add(new ConsumesAttribute("application/json"));
})
 .AddXmlSerializerFormatters();


//Enable versioning in Web API controllers
builder.Services.AddApiVersioning(config =>
{
 config.ApiVersionReader = new UrlSegmentApiVersionReader(); //Reads version number from request url at "apiVersion" constraint

 //config.ApiVersionReader = new QueryStringApiVersionReader(); //Reads version number from request query string called "api-version". Eg: api-version=1.0

 //config.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Reads version number from request header called "api-version". Eg: api-version: 1.0

 config.DefaultApiVersion = new ApiVersion(1, 0);
 config.AssumeDefaultVersionWhenUnspecified = true;
});




builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
 options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


//Swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints


builder.Services.AddSwaggerGen(options => {
 options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
}); //generates OpenAPI specification



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger(); //creates endpoint for swagger.json
app.UseSwaggerUI(); //creates swagger UI for testing all Web API endpoints / action methods

app.UseAuthorization();

app.MapControllers();

app.Run();
