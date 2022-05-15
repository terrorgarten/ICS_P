using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Common.Enums;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.DAL.Tests;

public class DbContextUserTests : DbContextTestsBase
{
    public DbContextUserTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_UserWithoutCars_Persisted()
    {
        //Arrange
        var entity = UserSeeds.EmptyUserEntityWithourCars with
        {
            Name = "Vladimir",
            Surname = "Zelensky"
        };

        //Act
        CarpoolDbContextSUT.Users.Add(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_UserWithCars_Persisted()
    {
        //Arrange
        var entity = UserSeeds.EmptyUserEntity with
        {
            Id = Guid.Parse("BC43C200-AFA2-44FB-AFC7-B4149C293580"),
            Name = "Ronald",
            Surname = "Reagan",
            OwnedCars = new List<CarEntity>
            {
                CarSeeds.EmptyCarEntity with
                {
                    Manufacturer = Manufacturer.AlfaRomeo,
                    CarType = CarType.Targa,
                    RegistrationDate = new DateTime(2020, 10, 7),
                    PhotoUrl = null,
                    SeatCapacity = 2
                },
                CarSeeds.EmptyCarEntity with
                {
                    Manufacturer = Manufacturer.Volkswagen,
                    CarType = CarType.Van,
                    RegistrationDate = new DateTime(2021, 7, 14),
                    PhotoUrl = null,
                    SeatCapacity = 8
                }
            }
        };

        //Act
        CarpoolDbContextSUT.Users.Add(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .Include(i => i.OwnedCars)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity, "PassengerRides");
    }


    [Fact]
    public async Task GetById_User()
    {
        //Act
        var entity = await CarpoolDbContextSUT.Users
            .SingleAsync(i => i.Id == UserSeeds.UserEntity.Id);

        //Assert
        DeepAssert.Equal(
            UserSeeds.UserEntity with
            {
                OwnedCars = Array.Empty<CarEntity>(), PassengerRides = Array.Empty<UserRideEntity>()
            }, entity);
    }

    [Fact]
    public async Task GetById_IncludingCars_User()
    {
        //Act
        var entity = await CarpoolDbContextSUT.Users
            .Include(i => i.OwnedCars)
            .SingleAsync(i => i.Id == UserSeeds.UserEntity1.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.UserEntity1 with { PassengerRides = Array.Empty<UserRideEntity>() }, entity);
    }

    [Fact]
    public async Task Update_User_Persisted()
    {
        //Arrange
        var baseEntity = UserSeeds.UserEntityUpdate;
        var entity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated",
                Surname = baseEntity.Surname + "Updated"
            };

        //Act
        CarpoolDbContextSUT.Users.Update(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }


    [Fact]
    public async Task Delete_User_Deleted()
    {
        //Arrange
        var baseEntity = UserSeeds.UserEntityDelete;

        //Act
        CarpoolDbContextSUT.Users.Remove(baseEntity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_User_Deleted()
    {
        //Arrange
        var baseEntity = UserSeeds.UserEntityDelete;

        //Act
        CarpoolDbContextSUT.Remove(
            CarpoolDbContextSUT.Users.Single(i => i.Id == baseEntity.Id));
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    }
}