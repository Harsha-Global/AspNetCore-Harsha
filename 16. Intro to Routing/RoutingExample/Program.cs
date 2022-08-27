var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints =>
{
  //add your endpoints here

});
app.Run();
