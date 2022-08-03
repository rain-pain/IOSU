using IOSU.Models;
using Microsoft.EntityFrameworkCore;

namespace IOSU.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<IOSU.Models.Manufacturer> Manufacturer { get; set; }
        public DbSet<IOSU.Models.Contract> Contract { get; set; }
        public DbSet<IOSU.Models.Product> Product { get; set; }
        public DbSet<IOSU.Models.Manager> Manager { get; set; }
        public DbSet<IOSU.Models.Client> Client { get; set; }
    }
}
