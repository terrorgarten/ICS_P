using Carpool.DAL.Entities;
using Carpool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

//Seeds, tests and this file are in progress according to Demo 2
namespace Carpool.DAL;

public class CarpoolDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public CarpoolDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions)
    {
        _seedDemoData = seedDemoData;
    }

    public DbSet<CarEntity> Cars => Set<CarEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RideEntity> Rides => Set<RideEntity>();
    public DbSet<UserRideEntity> UsersRideEntity => Set<UserRideEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.OwnedCars)
            .WithOne(i => i.Owner)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<CarEntity>()
            .HasMany(i => i.Rides)
            .WithOne(i => i.Car)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.PassengerRides)
            .WithOne(i => i.Passenger)
            .OnDelete(DeleteBehavior
                .Cascade); //opraveno na NoAction, kvoli Erroru z migracie a nasledne update-database, logicky sa nam javi Cascade

        modelBuilder.Entity<RideEntity>()
            .HasMany(i => i.PassengerRides)
            .WithOne(i => i.Ride)
            .OnDelete(DeleteBehavior.Cascade);

        if (!_seedDemoData) return;
        UserSeeds.Seed(modelBuilder);
        CarSeeds.Seed(modelBuilder);
        RideSeeds.Seed(modelBuilder);
        UserRideSeeds.Seed(modelBuilder);
    }
}