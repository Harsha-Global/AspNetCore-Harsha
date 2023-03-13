using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext
{
 public class ApplicationDbContext : DbContext
 {
  public ApplicationDbContext(DbContextOptions options) : base(options) 
  { 
  }

  public ApplicationDbContext()
  {
  }

  public virtual DbSet<City> Cities { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   base.OnModelCreating(modelBuilder);

   modelBuilder.Entity<City>().HasData(
    new City() { CityID = Guid.Parse("BF160CFD-E693-4C6A-9417-037B4501EC9B"), CityName = "New York" });

   modelBuilder.Entity<City>().HasData(
    new City() { CityID = Guid.Parse("858462EF-5587-48D5-8DB3-392938699F42"), CityName = "London" });
  }
 }
}
