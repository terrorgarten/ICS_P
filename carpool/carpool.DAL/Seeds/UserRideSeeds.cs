using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds
{
    public static class UserRideSeeds
    {
        public static readonly UserRideEntity UserRide1 = new(
            Id: Guid.Parse(input: "3C5391D1-624E-4CEB-A93C-1A6F4980CEF7"),
            PassengerId: UserSeeds.FirstUser.Id,
            RideId: RideSeeds.Ride1.Id)
        {
            Passenger = UserSeeds.FirstUser,
            Ride = RideSeeds.Ride1
        };

        public static readonly UserRideEntity UserRide2 = new(
            Id: Guid.Parse(input: "E29C9F5C-F879-4BD9-9B3D-0AA5327E6730"),
            PassengerId: UserSeeds.SecondUser.Id,
            RideId: RideSeeds.Ride2.Id)
        {
            Passenger = UserSeeds.SecondUser,
            Ride = RideSeeds.Ride2
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRideEntity>().HasData(
            UserRide1 with { Passenger = null, Ride = null },
            UserRide2 with { Passenger = null, Ride = null }
            );
        }
    }
}
