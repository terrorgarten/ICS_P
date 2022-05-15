using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Seeds;

public static class UserRideSeeds
{
    public static readonly UserRideEntity EmptyUserRideEntity = new(
        default,
        default,
        default)
    {
        Passenger = default,
        Ride = default
    };

    public static readonly UserRideEntity UserRideEntity1 = new(
        Guid.Parse("515D43D0-B60F-4CAC-BAE2-88362A410950"),
        UserSeeds.UserForUserRideEntityUpdate.Id,
        RideSeeds.RideEntityForUserRideEntity.Id)
    {
        Passenger = UserSeeds.UserForUserRideEntityUpdate,
        Ride = RideSeeds.RideEntityForUserRideEntity
    };

    public static readonly UserRideEntity UserRideEntity2 = new(
        Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        UserSeeds.UserForUserRideEntity.Id,
        RideSeeds.RideEntityForRideTestsGet.Id)
    {
        Passenger = UserSeeds.UserForUserRideEntity,
        Ride = RideSeeds.RideEntityForRideTestsGet
    };

    public static readonly UserRideEntity UserRideEntity3 = new(
        Guid.Parse("0D6AD977-11EA-424F-982D-45C3F0BC1CC2"),
        UserSeeds.UserEntity2.Id,
        RideSeeds.RideEntityForUserRideEntity.Id)
    {
        Passenger = UserSeeds.UserEntity2,
        Ride = RideSeeds.RideEntityForUserRideEntity
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly UserRideEntity UserRideEntityUpdate = UserRideEntity1 with
    {
        Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Ride = null, Passenger = null,
        PassengerId = UserSeeds.UserForUserRideEntityUpdate.Id
    };

    public static readonly UserRideEntity UserRideEntityDelete = UserRideEntity2 with
    {
        Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), Ride = null, Passenger = null,
        RideId = RideSeeds.RideEntityForRideUserDelete.Id
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRideEntity>().HasData(
            UserRideEntity1 with { Passenger = null, Ride = null },
            UserRideEntity2 with { Passenger = null, Ride = null },
            UserRideEntity3 with { Passenger = null, Ride = null },
            UserRideEntityUpdate,
            UserRideEntityDelete
        );
    }
}