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

 options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });

 options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });

}); //generates OpenAPI specification


builder.Services.AddVersionedApiExplorer(options => {
 options.GroupNameFormat = "'v'VVV"; //v1
 options.SubstituteApiVersionInUrl = true;
});

//CORS: localhost:4200
builder.Services.AddCors(options => {
 options.AddDefaultPolicy(builder =>
 {
  builder.WithOrigins("http://localhost:4200");
 });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger(); //creates endpoint for swagger.json
app.UseSwaggerUI(options  =>
{
 options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
 options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
}); //creates swagger UI for testing all Web API endpoints / action methods
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
