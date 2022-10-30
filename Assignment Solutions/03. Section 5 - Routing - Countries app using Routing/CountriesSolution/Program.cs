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
 //when request path is "/countries"
 endpoints.MapGet("/countries", async context =>
 {
  //read all countries
  foreach (KeyValuePair<int, string> country in countries)
  {
   //write country details to response
   await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
  }
 });


 //When request path is "countries/{countryID}"
 endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async context =>
 {
  //check if "countryID" was not submitted in the request
  if (context.Request.RouteValues.ContainsKey("countryID") == false)
  {
   context.Response.StatusCode = 400;
   await context.Response.WriteAsync("The CountryID should be between 1 and 100");
  }


  //read countryID from RouteValues (route parameters)
  int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);


  //if the countryID exists in the countries dictionary
  if (countries.ContainsKey(countryID))
  {
   string countryName = countries[countryID];

   //write country name to response
   await context.Response.WriteAsync($"{countryName}");
  }

  //if countryID not exists in countries dictionary
  else
  {
   context.Response.StatusCode = 404;
   await context.Response.WriteAsync($"[No country]");
  }
 });

 //When request path is "countries/{countryID}"
 endpoints.MapGet("/countries/{countryID:min(101)}", async context =>
 {
  context.Response.StatusCode = 400;
  await context.Response.WriteAsync("The CountryID should be between 1 and 100 - min");
 });

});

//Default middleware
app.Run(async context =>
{
 await context.Response.WriteAsync("No response");
});

app.Run();
