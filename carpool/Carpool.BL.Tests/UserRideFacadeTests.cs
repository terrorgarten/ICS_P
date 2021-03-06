using System;
using Carpool.BL.Facades;
using Carpool.BL.Models;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.BL.Tests;

public sealed class UserRideFacadeTests : CRUDFacadeTestsBase
{
    private readonly UserRideFacade _userRideFacadeSUT;

    public UserRideFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userRideFacadeSUT = new UserRideFacade(UnitOfWorkFactory, Mapper);
    }
    
    [Fact]
    public async Task Create_WithNonExistingItem_UserRideDetail_DoesNotThrow()
    {
        var _ = new UserRideDetailModel
        (
            UserSeeds.UserEntity1.Name,
            UserSeeds.UserEntity1.Surname
        )
        {
            RideId = RideSeeds.RideEntity.Id,
            PassengerId = UserSeeds.UserEntity1.Id
        };


        await _userRideFacadeSUT.SaveCheckAsync(UserSeeds.UserEntity1.Id, RideSeeds.RideEntity.Id);
    }

    [Fact]
    public async Task GetAll_Single_SeededUserRide()
    {
        var users = await _userRideFacadeSUT.GetAsync();
        var user = users.Single(i => i.Id == UserRideSeeds.UserRideEntity1.Id);

        DeepAssert.Equal(Mapper.Map<UserRideDetailModel>(UserRideSeeds.UserRideEntity1), user);
    }

    [Fact]
    public async Task Insert_SeededUserRide()
    {
        var seeded_user = new UserRideDetailModel
        (
            UserSeeds.UserEntity1.Name,
            UserSeeds.UserEntity1.Surname
        )
        {
            RideId = RideSeeds.RideEntity.Id,
            PassengerId = UserSeeds.UserEntity1.Id
        };
        seeded_user = await _userRideFacadeSUT.SaveAsync(seeded_user);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var userRideFromDb = await dbxAssert.UsersRideEntity.SingleAsync(i => i.Id == seeded_user.Id);
        DeepAssert.Equal(seeded_user, Mapper.Map<UserRideDetailModel>(userRideFromDb), "Name", "Surname");
    }


    [Fact]
    public async Task GetById_SeededUserRide()
    {
        var user = await _userRideFacadeSUT.GetAsync(UserRideSeeds.UserRideEntity1.Id);

        DeepAssert.Equal(Mapper.Map<UserRideDetailModel>(UserRideSeeds.UserRideEntity1), user);
    }

    [Fact]
    public async Task GetById_NonExistentUserRide()
    {
        var user = await _userRideFacadeSUT.GetAsync(UserRideSeeds.EmptyUserRideEntity.Id);

        Assert.Null(user);
    }

    [Fact]
    public async Task SeededUser_DeleteById_Deleted()
    {
        await _userRideFacadeSUT.DeleteAsync(UserRideSeeds.UserRideEntity1.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserRideSeeds.UserRideEntity1.Id));
    }
}