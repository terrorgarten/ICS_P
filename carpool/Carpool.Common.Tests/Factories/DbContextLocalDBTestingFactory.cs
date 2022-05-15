using Carpool.DAL;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Factories;

public class DbContextLocalDBTestingFactory : IDbContextFactory<CarpoolDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextLocalDBTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public CarpoolDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<CarpoolDbContext> builder = new();
        builder.UseSqlServer(
            $"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = {_databaseName};MultipleActiveResultSets = True;Integrated Security = True; ");


        return new CarpoolTestingDbContext(builder.Options, _seedTestingData);
    }
}