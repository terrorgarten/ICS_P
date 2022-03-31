using System;
using Microsoft.EntityFrameworkCore;

namespace carpool.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<CarpoolDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<CarpoolDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}