﻿using Carpool.Common.Tests.Seeds;
using Carpool.DAL;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests;

public class CarpoolTestingDbContext : CarpoolDbContext
{
    private readonly bool _seedTestingData;

    public CarpoolTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (!_seedTestingData) return;
        UserSeeds.Seed(modelBuilder);
        CarSeeds.Seed(modelBuilder);
        RideSeeds.Seed(modelBuilder);
        UserRideSeeds.Seed(modelBuilder);
    }
}