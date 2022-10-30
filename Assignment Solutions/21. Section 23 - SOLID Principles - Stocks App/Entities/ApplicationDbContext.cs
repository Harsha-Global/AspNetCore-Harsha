using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using System;
using System.Threading.Tasks;

namespace Entities
{
 public class ApplicationDbContext : DbContext
 {
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<BuyOrder> BuyOrders { get; set; }
  public DbSet<SellOrder> SellOrders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   base.OnModelCreating(modelBuilder);

   modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
   modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
  }
 }
}

