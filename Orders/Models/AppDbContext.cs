using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Orders> Orders { get; set; }
    }
}
