using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Seeds;

public static class UserRideSeeds
{
    public static readonly UserRideEntity EmptyUserRideEntity = new(
        Id: default,
        PassengerId: default,
        RideId: default)
    {
        Passenger = default,
        Ride = default
    };

    public static readonly UserRideEntity UserRideEntity1 = new(
        Id: Guid.Parse(input: "515D43D0-B60F-4CAC-BAE2-88362A410950"),
        PassengerId: UserSeeds.UserEntity1.Id,
        RideId: RideSeeds.RideEntity1.Id)
    {
        Passenger = UserSeeds.UserEntity1,
        Ride = RideSeeds.RideEntity1
    };

    public static readonly UserRideEntity UserRideEntity2 = new(
        Id: Guid.Parse(input: "87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        PassengerId: UserSeeds.UserEntity2.Id,
        RideId: RideSeeds.RideEntity2.Id)
    {
        Passenger = UserSeeds.UserEntity2,
        Ride = RideSeeds.RideEntity2
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    //public static readonly UserRideEntity UserRideEntityUpdate = UserRideEntity1 with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Ride = null, Passenger = null, PassengerId = UserSeeds.UserForUserRideEntityUpdate.Id };
    //public static readonly UserRideEntity UserRideEntityDelete = UserRideEntity2 with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), Ride = null, Passenger = null, RideId = UserSeeds.UserForUserRideEntityDelete.Id };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRideEntity>().HasData(
            UserRideEntity1 with { Passenger = null, Ride = null },
            UserRideEntity2 with { Passenger = null, Ride = null }
            //UserRideEntityUpdate,
            //UserRideEntityDelete
        );
    }
}
