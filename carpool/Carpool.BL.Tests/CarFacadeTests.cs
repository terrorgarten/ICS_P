using Carpool.BL.Models;
using System;
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
    public sealed class CarFacadeTests : CRUDFacadeTestsBase
    {
        private readonly CarFacade _carFacadeSUT;

        public CarFacadeTests(ITestOutputHelper output) : base(output)
        {
            _carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new CarDetailModel
            (
                CarType: CarType.Coupe,
                Manufacturer: Manufacturer.Subaru,
                SeatCapacity: 4,
                RegistrationDate: DateTime.MaxValue,
                OwnerId: UserSeeds.UserEntity1.Id
            )
            {
                //Id = Guid.Parse("953C5784-DB27-43D2-A8F3-ED6E6C58CDE4")
            };
            
            var _ = await _carFacadeSUT.SaveAsync(model);
            
        }

        [Fact]
        public async Task GetAll_Single_SeededSportCar()
        {

            var cars = await _carFacadeSUT.GetAsync();
            var car = cars.Single(i => i.Id == CarSeeds.SportCar.Id);

            DeepAssert.Equal(Mapper.Map<CarListModel>(CarSeeds.SportCar), car);
        }

        [Fact]
        public async Task GetById_SeededSportCar()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.SportCar.Id);

            DeepAssert.Equal(Mapper.Map<CarDetailModel>(CarSeeds.SportCar), car);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.EmptyCarEntity.Id);

            Assert.Null(car);
        }

        [Fact]
        public async Task SeededSportCar_DeleteById_Deleted()
        {
            await _carFacadeSUT.DeleteAsync(CarSeeds.SportCar.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeeds.SportCar.Id));
        }

        [Fact]
        public async Task NewCar_InsertOrUpdate_CarAdded()
        {
            //Arrange
            var car = new CarDetailModel(
                CarType: CarType.SUV,
                Manufacturer: Manufacturer.Chevrolet,
                SeatCapacity: 4,
                RegistrationDate: DateTime.MaxValue,
                OwnerId: UserSeeds.UserEntity1.Id
            );

            //Act
            car = await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.Id);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }

        [Fact]
        public async Task SeededSportCar_InsertOrUpdate_CarUpdated()
        {
            //Arrange
            var car = new CarDetailModel
            (
                CarType: CarSeeds.SportCar.CarType,
                Manufacturer: CarSeeds.SportCar.Manufacturer,
                SeatCapacity: CarSeeds.SportCar.SeatCapacity,
                RegistrationDate: CarSeeds.SportCar.RegistrationDate,
                OwnerId: CarSeeds.SportCar.OwnerId
            )
            {
                Id = CarSeeds.SportCar.Id
            };

            // Update CarType and Manufacturer
            car.CarType = CarType.Minivan;
            car.Manufacturer = Manufacturer.Bugatti;

            //Act
            await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.Id);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }
    }
}
