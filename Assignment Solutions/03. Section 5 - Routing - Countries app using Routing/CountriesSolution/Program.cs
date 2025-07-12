var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

//data
Dictionary<int, string> countries = new Dictionary<int, string>()
{
 { 1, "United States" },
 { 2, "Canada" },
 { 3, "United Kingdom" },
 { 4, "India" },
 { 5, "Japan" }
};

//endpoints
app.UseEndpoints(endpoints =>
{
 //Route 1: when request path is "/countries" (list all countries)
 endpoints.MapGet("/countries", async context =>
 {
  foreach (KeyValuePair<int, string> country in countries)
  {
   await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
  }
 });

 //Route 2: when request path is "/countries/{countryID}" for IDs 1-100
 endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async context =>
 {
  //read countryID from RouteValues
  int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);
  
  //if the countryID exists in the countries dictionary
  if (countries.ContainsKey(countryID))
  {
   string countryName = countries[countryID];
   await context.Response.WriteAsync($"{countryName}");
  }
  //if countryID doesn't exist in countries dictionary (but is 1-100 range)
  else
  {
   context.Response.StatusCode = 404;
   await context.Response.WriteAsync("[No Country]");
  }
 });

 //Route 3: when request path is "/countries/{countryID}" for IDs greater than 100
 endpoints.MapGet("/countries/{countryID:int:min(101)}", async context =>
 {
  context.Response.StatusCode = 400;
  await context.Response.WriteAsync("The CountryID should be between 1 and 100");
 });
});

//Default middleware
app.Run(async context =>
{
 await context.Response.WriteAsync("No response");
});

app.Run();
