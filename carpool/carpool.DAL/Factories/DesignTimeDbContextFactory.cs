using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace carpool.DAL.Factories
{
    /// <summary>
    /// EF Core CLI migration generation uses this DbContext to create model and migration
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarpoolDbContext>
    {
        public CarpoolDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<CarpoolDbContext> builder = new();
            builder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog = CookBook;
                MultipleActiveResultSets = True;
                Integrated Security = True; ");

            return new CarpoolDbContext(builder.Options);
        }
    }
}
