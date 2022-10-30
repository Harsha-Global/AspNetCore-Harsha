using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample
{
 public static class ConfigureServicesExtension
 {
  public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
  {
   services.AddTransient<ResponseHeaderActionFilter>();

   //it adds controllers and views as services
   services.AddControllersWithViews(options => {
    //options.Filters.Add<ResponseHeaderActionFilter>(5);

    var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

    options.Filters.Add(new ResponseHeaderActionFilter(logger)
    {
     Key = "My-Key-From-Global",
     Value = "My-Value-From-Global",
     Order = 2
    });
   });

   //add services into IoC container
   services.AddScoped<ICountriesRepository, CountriesRepository>();
   services.AddScoped<IPersonsRepository, PersonsRepository>();

   services.AddScoped<ICountriesGetterService, CountriesGetterService>();
   services.AddScoped<ICountriesAdderService, CountriesAdderService>();
   services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

   services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();
   services.AddScoped<PersonsGetterService, PersonsGetterService>();

   services.AddScoped<IPersonsAdderService, PersonsAdderService>();
   services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
   services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
   services.AddScoped<IPersonsSorterService, PersonsSorterService>();

   services.AddDbContext<ApplicationDbContext>(options =>
   {
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
   });

   services.AddTransient<PersonsListActionFilter>();

   services.AddHttpLogging(options =>
   {
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
   });

   return services;
  }
 }
}
