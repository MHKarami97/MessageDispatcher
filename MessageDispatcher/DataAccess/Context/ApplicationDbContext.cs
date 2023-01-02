using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
