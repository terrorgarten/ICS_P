using System;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Common.Enums;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.DAL.Tests
{
    /// <summary>
    /// Tests shows an example of DbContext usage when querying strong entity with no navigation properties.
    /// Entity has no relations, holds no foreign keys.
    /// </summary>
    public class DbContextCarTests : DbContextTestsBase
    {
        public DbContextCarTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AddNew_Car_Persisted()
        {
            //Arrange
            var entity = CarSeeds.EmptyCarEntity with
            {
                Manufacturer = Manufacturer.Dacia,
                CarType = CarType.Universal,
                OwnerId = UserSeeds.UserEntity.Id
            };

            //Act
            CarpoolDbContextSUT.Cars.Add(entity);
            await CarpoolDbContextSUT.SaveChangesAsync();

            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Cars.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntities);
        }

        [Fact]
        public async Task AddNew_CarWithOwner_Persisted()
        {
            //Arrange
            var entity = CarSeeds.EmptyCarEntity with
            {
                Manufacturer = Manufacturer.Lamborghini,
                CarType = CarType.Cabriolet,
                OwnerId = Guid.Parse("39059CAC-0E34-4B2E-BC3C-4044CC4897F2"),
                Owner = UserSeeds.EmptyUserEntity with
                {
                    Id = Guid.Parse("39059CAC-0E34-4B2E-BC3C-4044CC4897F2"),
                    Name = "Stanko",
                    Surname = "Lobotka"
                }
            };

            //Act
            CarpoolDbContextSUT.Cars.Add(entity);
            await CarpoolDbContextSUT.SaveChangesAsync();

            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Cars.Include(i => i.Owner).SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntities, "PassengerRides", "OwnerId");
        }

        [Fact]
        public async Task Update_SportCar_Persisted()
        {
            //Arrange
            var baseEntity = CarSeeds.CarEntityUpdate;
            var entity = CarSeeds.CarEntityUpdate with
            {
                SeatCapacity = baseEntity.SeatCapacity + 1,
            };

            //Act
            CarpoolDbContextSUT.Cars.Update(entity);
            await CarpoolDbContextSUT.SaveChangesAsync();

            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntity = await dbx.Cars.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntity);
        }

       
        [Fact]
        public async Task GetById_IncludingOwner_Car()
        {
            //Act
            var entity = await CarpoolDbContextSUT.Cars
                .Include(i => i.Owner)
                .ThenInclude(i => i!.OwnedCars)
                .SingleAsync(i => i.Id == CarSeeds.CarEntity1.Id);

            //Assert
            DeepAssert.Equal(CarSeeds.CarEntity1, entity);
        }

        [Fact]
        public async Task Delete_SportCar_Deleted()
        {
            //Arrange
            var entityBase = CarSeeds.CarEntityDelete;

            //Act
            CarpoolDbContextSUT.Cars.Remove(entityBase);
            await CarpoolDbContextSUT.SaveChangesAsync();

            //Assert
            Assert.False(await CarpoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
        }

        [Fact]
        public async Task DeleteById_Car_SportCarDeleted()
        {
            //Arrange
            var entityBase = CarSeeds.CarEntityDelete;

            //Act
            CarpoolDbContextSUT.Remove(
                CarpoolDbContextSUT.Cars.Single(i => i.Id == entityBase.Id));
            await CarpoolDbContextSUT.SaveChangesAsync();

            //Assert
            Assert.False(await CarpoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
        }
    }
}
