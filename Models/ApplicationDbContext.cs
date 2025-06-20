using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

/*
 * ModelBuilder class provides a simple API surface for configuring IMutableModel
 * that defines the shape of the entitities, relationships between them, and how
 * they map to the DB (SQLite in this instance).
 * 
 * IMutableModel interface provides metadata for the functionality of the ModelBuilder 
 * (shape, relationships, and DB mapping)
 */


namespace JobRecorderNet.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
                : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Evidence> Evidence { get; set; }


        // Needs OnModelCreating method to configure relationships between ALL entities in the SQLite database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // To ensure that the DB schema is created correctly with the default config
            base.OnModelCreating(modelBuilder);
        }

    }
}
