using System;
using Carpool.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.BL.Tests
{
    public sealed class RideFacadeTests : CRUDFacadeTestsBase
    {
        private readonly RideFacade _rideFacadeSUT;

        public RideFacadeTests(ITestOutputHelper output) : base(output)
        {
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }
        [Fact]
        public async Task Create_WithNonExistingItemDoesNotThrow()
        {
            var model = new RideDetailModel
            (
                Start: "Praha",
                End: "Brno",
                BeginTime: DateTime.MaxValue,
                ApproxRideTime: TimeSpan.MaxValue,
                CarId: CarSeeds.CarEntity1.Id,
                UserId: UserSeeds.UserEntity1.Id
            )
            {
            };

            var _ = await _rideFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task GetAll_Single_SeededRideEntity()
        {
            var rides = await _rideFacadeSUT.GetAsync();
            var ride = rides.Single(i => i.Id == RideSeeds.RideEntity.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeeds.RideEntity), ride);
        }

        
        [Fact]
        public async Task GetById_SeededRideEntity()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            DeepAssert.Equal(Mapper.Map<RideDetailModel>(RideSeeds.RideEntity), ride, "OwnedCars");
        }
        
        [Fact]
        public async Task GetById_NonExistent()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.EmptyRideEntity.Id);

            Assert.Null(ride);
        }
        
        [Fact]
        public async Task SeededRideEntity_DeleteById_Deleted()
        {
            await _rideFacadeSUT.DeleteAsync(RideSeeds.RideEntity.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Rides.AnyAsync(i => i.Id == RideSeeds.RideEntity.Id));
        }

        [Fact]
        public async Task NewRide_InsertOrUpdate_RideAdded()
        {
            //Arrange
            var ride = new RideDetailModel(
                Start: "Madrid",
                End: "Berlin",
                BeginTime: DateTime.MaxValue,
                ApproxRideTime: TimeSpan.MaxValue,
                CarId: CarSeeds.CarEntityUpdate.Id,
                UserId: UserSeeds.UserEntityUpdate.Id
            );

            //Act
            ride = await _rideFacadeSUT.SaveAsync(ride);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var rideFromDb = await dbxAssert.Rides.SingleAsync(i => i.Id == ride.Id);
            DeepAssert.Equal(ride, Mapper.Map<RideDetailModel>(rideFromDb), "User", "Car");
        }

        [Fact]
        public async Task SeededRideEntity_InsertOrUpdate_RideUpdated()
        {
            //Arrange
            var ride = new RideDetailModel
            (
                Start: RideSeeds.RideEntity.Start,
                End: RideSeeds.RideEntity.End,
                BeginTime: RideSeeds.RideEntity.BeginTime,
                ApproxRideTime: RideSeeds.RideEntity.ApproxRideTime,
                CarId: CarSeeds.CarEntity1.Id,
                UserId: UserSeeds.UserEntity1.Id
            )
            {
                Id = RideSeeds.RideEntity.Id
            };
            ride.Start = "Liberec";
            ride.End = "Ostrava";

            //Act
            await _rideFacadeSUT.SaveAsync(ride);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var rideFromDb = await dbxAssert.Rides.SingleAsync(i => i.Id == ride.Id);
            DeepAssert.Equal(ride, Mapper.Map<RideDetailModel>(rideFromDb));
        }
    }
}