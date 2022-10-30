using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace CRUDTests
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
