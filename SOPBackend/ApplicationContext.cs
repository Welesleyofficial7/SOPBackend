namespace SOPBackend;

using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<MenuItem> MenuItems { get; set; }
    
    public DbSet<Promotion> Promotions { get; set; }
}
