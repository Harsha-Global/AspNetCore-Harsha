var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Routing is automatically enabled.
//No need for app.UseRouting() anymore

//Endpoints are defined directly on the "app" object.
//We will add endpoints here in the next lecture.

app.Run();
