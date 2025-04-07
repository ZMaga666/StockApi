using Microsoft.EntityFrameworkCore;
using StockApi.Model;

namespace StockApi.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Stock> stocks { get; set; }
    }
}
