using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// Interface is important for design-time services (EF Core tools) to create the DbContext
// DBContext is used to interact with the database (SQLite in this case), via the ModelBuilder class
namespace JobRecorderNet.Models
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=./Data/SQLiteDatabase.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
