using Microsoft.EntityFrameworkCore;
using OrderingSystem.Modules.Catalog.Entities;
using OrderingSystem.Modules.Ordering.Entities;

namespace OrderingSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderingSystem.Modules.Payments.Entities.Payment> Payments { get; set; }
}
