using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carpool.DAL.Entities;
using carpool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace carpool.DAL
{
    public class CarpoolDbContext : DbContext
    {
        public CarpoolDbContext(DbContextOptions contextOptions)
            : base(contextOptions)
        {
            
        }
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CarEntity> Cars => Set<CarEntity>();
        public DbSet<RideEntity> Rides => Set<RideEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CarSeeds.Seed(modelBuilder);
            UserSeeds.Seed(modelBuilder);
            RideSeeds.Seed(modelBuilder);
        }
    }
}
