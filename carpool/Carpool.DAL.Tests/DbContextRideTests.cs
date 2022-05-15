using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.DAL.Tests;

public class DbContextRideTests : DbContextTestsBase
{
    public DbContextRideTests(ITestOutputHelper output) : base(output)
    {
    }


    [Fact]
    public async Task AddNew_RideWithNoPassengers_Persisted()
    {
        //Arange
        var entity = RideSeeds.EmptyRideEntity with
        {
            Start = "Brno",
            End = "Praha",
            CarId = CarSeeds.CarEntity1.Id
        };

        //Act
        CarpoolDbContextSUT.Rides.Add(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();


        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Rides
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_RideWithPassengers_Persisted()
    {
        //Arrange
        var entity = RideSeeds.EmptyRideEntity with
        {
            Start = "Olomouc",
            End = "Hustopeče",
            BeginTime = new DateTime(2020, 10, 20, 10, 20, 0),
            ApproxRideTime = TimeSpan.FromHours(3),
            CarId = CarSeeds.CarEntity1.Id,

            PassengerRides = new List<UserRideEntity>
            {
                UserRideSeeds.EmptyUserRideEntity with
                {
                    Passenger = UserSeeds.EmptyUserEntity with
                    {
                        Name = "Franta",
                        Surname = "Blazen"
                    }
                },

                UserRideSeeds.EmptyUserRideEntity with
                {
                    Passenger = UserSeeds.EmptyUserEntity with
                    {
                        Name = "Jan",
                        Surname = "Kral"
                    }
                }
            }
        };

        //Act
        CarpoolDbContextSUT.Rides.Add(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Rides
            .Include(i => i.PassengerRides)
            .ThenInclude(i => i.Passenger).ThenInclude(i => i!.OwnedCars)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }


    [Fact]
    public async Task AddNew_RideWithPassengerRides_Persisted()
    {
        var entity = RideSeeds.EmptyRideEntity with
        {
            Start = "Olomouc",
            End = "Hustopeče",
            BeginTime = new DateTime(2020, 10, 20, 10, 20, 0),
            ApproxRideTime = TimeSpan.FromHours(3),
            CarId = CarSeeds.CarEntity1.Id,
            PassengerRides = new List<UserRideEntity>
            {
                UserRideSeeds.EmptyUserRideEntity with
                {
                    PassengerId = UserSeeds.UserEntity1.Id
                },

                UserRideSeeds.EmptyUserRideEntity with
                {
                    PassengerId = UserSeeds.UserEntity2.Id
                }
            }
        };

        //Act
        CarpoolDbContextSUT.Rides.Add(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Rides
            .Include(i => i.PassengerRides)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }


    [Fact]
    public async Task Update_Ride_Persisted()
    {
        //Arrange
        var baseEntity = RideSeeds.RideEntityUpdate;
        var entity =
            baseEntity with
            {
                Start = baseEntity.Start + "Updated",
                End = baseEntity.End + "Updated",
                BeginTime = default
                //TODO: add others
            };

        //Act
        CarpoolDbContextSUT.Rides.Update(entity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Rides.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }


    [Fact]
    public async Task GetById_Ride()
    {
        var entity = await CarpoolDbContextSUT.Rides
            .SingleAsync(i => i.Id == RideSeeds.RideEntityWithNoPassengers.Id);

        DeepAssert.Equal(RideSeeds.RideEntityWithNoPassengers, entity);
    }

    [Fact]
    public async Task GetById_IncludingPassengers_Ride()
    {
        //Act
        var entity = await CarpoolDbContextSUT.Rides
            .Include(i => i.PassengerRides)
            .ThenInclude(i => i.Passenger)
            .SingleAsync(i => i.Id == RideSeeds.RideEntityForRideTestsGet.Id);
        var passengerides = entity.PassengerRides;
        //Assert
        DeepAssert.Equal(RideSeeds.RideEntityForRideTestsGet with { PassengerRides = passengerides }, entity);
    }

    [Fact]
    public async Task Delete_PassengerRide_Deleted()
    {
        //Arrange
        var baseEntity = RideSeeds.RideEntityDelete;

        //Act
        CarpoolDbContextSUT.Rides.Remove(baseEntity);
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.Rides.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_PassengerRide_Deleted()
    {
        //Arrange
        var baseEntity = RideSeeds.RideEntityDelete;

        //Act
        CarpoolDbContextSUT.Remove(
            CarpoolDbContextSUT.Rides.Single(i => i.Id == baseEntity.Id));
        await CarpoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarpoolDbContextSUT.Rides.AnyAsync(i => i.Id == baseEntity.Id));
    }
}