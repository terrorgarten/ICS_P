using Carpool.Common.Enums;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds
{
    public static class UserRideSeeds
    {
        public static readonly UserRideEntity UserRide1 = new(
            Id: Guid.Parse(input: "C9ED341C-13F3-4A63-8CEA-7FC776060965"),
            PassengerId: UserSeeds.FirstUser.Id,
            RideId: RideSeeds.Ride1.Id)
        {
            Passenger = UserSeeds.FirstUser,
            Ride = RideSeeds.Ride1,
        };

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RideEntity>().HasData(UserRide1 with { Passenger = null, Ride = null});
        }
    }
}
