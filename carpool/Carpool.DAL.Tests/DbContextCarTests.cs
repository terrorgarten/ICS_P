//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Carpool.Common.Enums;
//using Carpool.Common.Tests;
//using Carpool.Common.Tests.Seeds;
//using Carpool.DAL.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Xunit;
//using Xunit.Abstractions;

//namespace Carpool.DAL.Tests
//{
//    /// <summary>
//    /// Tests shows an example of DbContext usage when querying strong entity with no navigation properties.
//    /// Entity has no relations, holds no foreign keys.
//    /// </summary>
//    public class DbContextCarTests : DbContextTestsBase
//    {
//        public DbContextCarTests(ITestOutputHelper output) : base(output)
//        {
//        }

//        [Fact]
//        public async Task Update_Car_Persisted()
//        {
//            //Arrange
//            var baseEntity = CarSeeds.CarEntityUpdate;
//            var entity = CarSeeds.CarEntityUpdate with
//            {
//                SeatCapacity = baseEntity.SeatCapacity + 1,
//            };

//            //Act
//            CarpoolDbContextSUT.Cars.Update(entity);
//            await CarpoolDbContextSUT.SaveChangesAsync();

//            //Assert
//            await using var dbx = await DbContextFactory.CreateDbContextAsync();
//            var actualEntity = await dbx.Cars.SingleAsync(i => i.Id == entity.Id);
//            Assert.Equal(entity, actualEntity);
//        }

//        [Fact]
//        public async Task GetAll_Car_ContainsSeededWater()
//        {
//            //Act
//            var entities = await CarpoolDbContextSUT.Cars.ToArrayAsync();

//            //Assert
//            Assert.Contains(CarSeeds.CarEntityUpdate, entities);
//        }

//        [Fact]
//        public async Task GetById_IncludingIngredients_Recipe()
//        {
//            //Act
//            var entity = await CarpoolDbContextSUT.Cars
//                .Include(i => i.Owner)
//                .ThenInclude(i => i.OwnedCars)
//                .SingleAsync(i => i.Id == CarSeeds.SportCar.Id);

//            //Assert
//            DeepAssert.Equal(CarSeeds.SportCar, entity);
//        }

//        [Fact]
//        public async Task Delete_Ingredient_WaterDeleted()
//        {
//            //Arrange
//            var entityBase = CarSeeds.CarEntityDelete;

//            //Act
//            CarpoolDbContextSUT.Cars.Remove(entityBase);
//            await CarpoolDbContextSUT.SaveChangesAsync();

//            //Assert
//            Assert.False(await CarpoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
//        }

//        [Fact]
//        public async Task DeleteById_Ingredient_WaterDeleted()
//        {
//            //Arrange
//            var entityBase = CarSeeds.CarEntityDelete;

//            //Act
//            CarpoolDbContextSUT.Remove(
//                CarpoolDbContextSUT.Cars.Single(i => i.Id == entityBase.Id));
//            await CarpoolDbContextSUT.SaveChangesAsync();

//            //Assert
//            Assert.False(await CarpoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
//        }
//    }
//}
