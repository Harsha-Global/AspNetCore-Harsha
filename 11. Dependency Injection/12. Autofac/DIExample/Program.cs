using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency(); //AddTransient

  containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope(); //AddScoped

  //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance(); //AddSingleton
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
