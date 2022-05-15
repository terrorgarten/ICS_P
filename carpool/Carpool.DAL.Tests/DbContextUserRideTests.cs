using System.Linq;
using System.Threading.Tasks;
using Carpool.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.DAL.Tests;

public class DbContextUserRideTests : DbContextTestsBase
{
    public DbContextUserRideTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Get_UserRide_ForRide1()
    {
        //Act
        var userRides = await CarpoolDbContextSUT.UsersRideEntity
            .Where(i => i.RideId == RideSeeds.RideEntityForUserRideEntity.Id)
            .ToArrayAsync();

        Assert.Contains(UserRideSeeds.UserRideEntity1 with
        {
            Ride = null,
            Passenger = null
        }, userRides);
    }

    [Fact]
    public async Task Get_UserRide_ForRide2()
    {
        //Act
        var userRides = await CarpoolDbContextSUT.UsersRideEntity
            .Where(i => i.RideId == RideSeeds.RideEntityForRideTestsGet.Id)
            .ToArrayAsync();

        Assert.Contains(UserRideSeeds.UserRideEntity2 with
        {
            Ride = null,
            Passenger = null
        }, userRides);
    }


    [Fact]
    public async Task Update_PassengerId_Persisted()
    {
        //Arrange
        var baseEntity = UserRideSeeds.UserRideEntityUpdate;
        var entity =
            baseEntity with
            {
                PassengerId = UserSeeds.UserEntity.Id //?
            };

        //Act
        CarpoolDbContextSUT.UsersRideEntity.Update(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.UsersRideEntity.SingleAsync(i => i.Id == entity.Id);
        Assert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Ride_UREDelete()
    {
        //Arrange
        var entityBase = RideSeeds.RideEntityForRideUserDelete;

        //Act
        CarpoolDbContextSUT.Rides.Remove(entityBase);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.Rides.AnyAsync(i => i.Id == entityBase.Id));
    }

    [Fact]
    public async Task Delete_UserRide_Deleted()
    {
        //Arrange
        var baseEntity = UserRideSeeds.UserRideEntityDelete;

        //Act
        CarpoolDbContextSUT.UsersRideEntity.Remove(baseEntity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.UsersRideEntity.AnyAsync(i => i.Id == baseEntity.Id));
    }
}