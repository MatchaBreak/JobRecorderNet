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
        // Inject database configuration
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
                : base(options)
        {
        }

        // To map to the Database (SQLite)
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

            // Job has one user supervisor
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Supervisor)
                .WithMany(u => u.SupervisedJobs)
                .HasForeignKey(j => j.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job has 1-to-many user technicians
            modelBuilder.Entity<Job>()
                .HasMany(j => j.Technicians)
                .WithMany(u => u.TechnicianJobs)
                .UsingEntity(j => j.ToTable("JobUser"));

            // Job has one address
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Address)
                .WithMany()
                .HasForeignKey(j => j.AddressId);

            // Job has no to many evidence
            modelBuilder.Entity<Job>()
                .HasMany(j => j.Evidences)
                .WithOne(e => e.Job)
                .HasForeignKey(e => e.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evidence has one Job
            modelBuilder.Entity<Evidence>()
                .HasOne(e => e.Job)
                .WithMany(Job => Job.Evidences)
                .HasForeignKey(e => e.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client has many addresses (1-to-many)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Client)
                .HasForeignKey("ClientId");

            // Client has many jobs (1-to-many)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Jobs)
                .WithOne(j => j.Client)
                .HasForeignKey(j => j.ClientId);

            // User has many licenses (1-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Licenses)
                .WithOne()
                .HasForeignKey("UserId");

            // User has one address 
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId) 
                .OnDelete(DeleteBehavior.Cascade); 
        }

    }
}
