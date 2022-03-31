using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carpool.DAL.Entities;
using carpool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

//Seeds, tests and this file are in progress according to Demo 2
namespace carpool.DAL
{
    public class CarpoolDbContext : DbContext
    {
        private readonly bool _seedDemoData;

        public CarpoolDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
            : base(contextOptions)
        {
            _seedDemoData = seedDemoData;
        }
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CarEntity> Cars => Set<CarEntity>();
        public DbSet<RideEntity> Rides => Set<RideEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.OwnedCars)
                .WithOne(i => i.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.DriverRides)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.PassengerRides)
                .WithOne(i => i.Passenger)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RideEntity>()
                .HasMany(i => i.PassengerRides)
                .WithOne(i => i.Ride)
                .OnDelete(DeleteBehavior.Restrict);


            if (!_seedDemoData) return;
            UserSeeds.Seed(modelBuilder);
            CarSeeds.Seed(modelBuilder);
            RideSeeds.Seed(modelBuilder);
        }
    }
}
