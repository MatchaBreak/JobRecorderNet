using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JobRecorderNet.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
                : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> jobs { get; set; }
        public DbSet<License> Licenses { get; set; }


    }
}
