using Carpool.DAL;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Factories;

public class DbContextTestingInMemoryFactory : IDbContextFactory<CarpoolDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public CarpoolDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<CarpoolDbContext> contextOptionsBuilder = new();
        contextOptionsBuilder.UseInMemoryDatabase(_databaseName);


        return new CarpoolTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}