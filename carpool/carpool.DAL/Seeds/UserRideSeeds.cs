using Carpool.Common.Enums;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds
{
    public static class UserRideSeeds
    {
        public static readonly UserRideEntity UserRide1 = new(
            Id: Guid.Parse(input: "DC935A57-A6B1-4806-B3D0-14FD166CA7AA"),
            PassengerId: UserSeeds.SecondUser.Id,
            RideId: RideSeeds.Ride1.Id)
        {
            Passenger = UserSeeds.SecondUser,
            Ride = RideSeeds.Ride1,
        };

        public static readonly UserRideEntity UserRide2 = new(
            Id: Guid.Parse(input: "F87FF297-C678-4AEA-B5DB-12AD3FDC6E87"),
            PassengerId: UserSeeds.FirstUser.Id,
            RideId: RideSeeds.Ride2.Id)
        {
            Passenger = UserSeeds.FirstUser,
            Ride = RideSeeds.Ride2,
        };

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RideEntity>().HasData(
                UserRide1 with { Passenger = null, Ride = null },
                UserRide2 with { Passenger = null, Ride = null });

        }
    }
}
