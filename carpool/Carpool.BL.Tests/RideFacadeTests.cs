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
    public sealed class IngredientFacadeTests : CRUDFacadeTestsBase
    {
        private readonly RideFacade _rideFacadeSUT;

        public IngredientFacadeTests(ITestOutputHelper output) : base(output)
        {
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }

        /*[Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new RideDetailModel
            (
                Start: "Praha",
                End: "Brno",
                BeginTime: DateTime.MaxValue,
                ApproxRideTime: TimeSpan.MaxValue, 
                Manufacturer: Manufacturer.Bugatti,
                SeatCapacity: 4,
                CarType: CarType.Minivan,
                UserId: UserSeeds.UserEntity.Id, //Franta
                CarId: CarSeeds.SportCar.Id
            )
            {
                
            };
            var _ = await _rideFacadeSUT.SaveAsync(model);
        }*/

        [Fact]
        public async Task GetAll_Single_SeededRideEntity()
        {
            var rides = await _rideFacadeSUT.GetAsync();
            var ride = rides.Single(i => i.Id == RideSeeds.RideEntity.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeeds.RideEntity), ride);
        }
        /*
        [Fact]
        public async Task GetById_SeededWater()
        {
            var ingredient = await _ingredientFacadeSUT.GetAsync(IngredientSeeds.Water.Id);

            DeepAssert.Equal(Mapper.Map<IngredientDetailModel>(IngredientSeeds.Water), ingredient);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var ingredient = await _ingredientFacadeSUT.GetAsync(IngredientSeeds.EmptyIngredient.Id);

            Assert.Null(ingredient);
        }

        [Fact]
        public async Task SeededWater_DeleteById_Deleted()
        {
            await _ingredientFacadeSUT.DeleteAsync(IngredientSeeds.Water.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Ingredients.AnyAsync(i => i.Id == IngredientSeeds.Water.Id));
        }


        [Fact]
        public async Task NewIngredient_InsertOrUpdate_IngredientAdded()
        {
            //Arrange
            var ingredient = new IngredientDetailModel(
                Name: "Water",
                Description: "Mineral water"
            );

            //Act
            ingredient = await _ingredientFacadeSUT.SaveAsync(ingredient);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var ingredientFromDb = await dbxAssert.Ingredients.SingleAsync(i => i.Id == ingredient.Id);
            DeepAssert.Equal(ingredient, Mapper.Map<IngredientDetailModel>(ingredientFromDb));
        }

        [Fact]
        public async Task SeededWater_InsertOrUpdate_IngredientUpdated()
        {
            //Arrange
            var ingredient = new IngredientDetailModel
            (
                Name: IngredientSeeds.Water.Name,
                Description: IngredientSeeds.Water.Description
            )
            {
                Id = IngredientSeeds.Water.Id
            };
            ingredient.Name += "updated";
            ingredient.Description += "updated";

            //Act
            await _ingredientFacadeSUT.SaveAsync(ingredient);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var ingredientFromDb = await dbxAssert.Ingredients.SingleAsync(i => i.Id == ingredient.Id);
            DeepAssert.Equal(ingredient, Mapper.Map<IngredientDetailModel>(ingredientFromDb));
        }*/
    }
}