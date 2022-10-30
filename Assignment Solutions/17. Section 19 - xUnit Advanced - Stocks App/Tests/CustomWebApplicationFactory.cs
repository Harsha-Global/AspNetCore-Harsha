using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using Microsoft.EntityFrameworkCore.InMemory;
using Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
 public class CustomWebApplicationFactory : WebApplicationFactory<Program>
 {
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
   base.ConfigureWebHost(builder);

   builder.UseEnvironment("Test");

   builder.ConfigureServices(services => {
    var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

    if (descripter != null)
    {
     services.Remove(descripter);
    }
    services.AddDbContext<ApplicationDbContext>(options =>
    {
     options.UseInMemoryDatabase("DatbaseForTesting");
    });
   });
  }
 }
}

