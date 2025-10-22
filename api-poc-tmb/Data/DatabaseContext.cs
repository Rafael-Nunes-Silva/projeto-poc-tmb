using api_poc_tmb.Models;
using Microsoft.EntityFrameworkCore;

namespace api_poc_tmb.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Order> orders { get; set; }
    }
}
