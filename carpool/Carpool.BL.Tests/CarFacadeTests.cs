using Carpool.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using Carpool.BL.Facades;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;

using System;
using System.Threading.Tasks;
using Carpool.BL.Facades;
using Carpool.BL.Models;
using Carpool.Common.Enums;
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
                CarType: CarType.Micro,
                Manufacturer: Manufacturer.Dacia,
                SeatCapacity: 4,
                RegistrationDate: DateTime.MaxValue
            );
            model.Id = Guid.Parse("33D3F8D8-4E50-43FE-A8BF-C3234A549976");
            var _ = await _carFacadeSUT.SaveAsync(model);
        }
    }
}
//        [Fact]
//        public async Task GetAll_Single_SeededWater()
//        {
//            var ingredients = await _ingredientFacadeSUT.GetAsync();
//                var ingredient = ingredients.Single(i => i.Id == IngredientSeeds.Water.Id);

//            DeepAssert.Equal(Mapper.Map<IngredientListModel>(IngredientSeeds.Water), ingredient);
//        }

//        [Fact]
//        public async Task GetById_SeededWater()
//        {
//            var ingredient = await _ingredientFacadeSUT.GetAsync(IngredientSeeds.Water.Id);

//            DeepAssert.Equal(Mapper.Map<IngredientDetailModel>(IngredientSeeds.Water), ingredient);
//        }

//        [Fact]
//        public async Task GetById_NonExistent()
//        {
//            var ingredient = await _ingredientFacadeSUT.GetAsync(IngredientSeeds.EmptyIngredient.Id);

//            Assert.Null(ingredient);
//        }

//        [Fact]
//        public async Task SeededWater_DeleteById_Deleted()
//        {
//            await _ingredientFacadeSUT.DeleteAsync(IngredientSeeds.Water.Id);

//            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
//            Assert.False(await dbxAssert.Ingredients.AnyAsync(i => i.Id == IngredientSeeds.Water.Id));
//        }


//        [Fact]
//        public async Task NewIngredient_InsertOrUpdate_IngredientAdded()
//        {
//            //Arrange
//            var ingredient = new IngredientDetailModel(
//                Name: "Water",
//                Description: "Mineral water"
//            );

//            //Act
//            ingredient = await _ingredientFacadeSUT.SaveAsync(ingredient);

//            //Assert
//            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
//            var ingredientFromDb = await dbxAssert.Ingredients.SingleAsync(i => i.Id == ingredient.Id);
//            DeepAssert.Equal(ingredient, Mapper.Map<IngredientDetailModel>(ingredientFromDb));
//        }

//        [Fact]
//        public async Task SeededWater_InsertOrUpdate_IngredientUpdated()
//        {
//            //Arrange
//            var ingredient = new IngredientDetailModel
//            (
//                Name: IngredientSeeds.Water.Name,
//                Description: IngredientSeeds.Water.Description
//            )
//            {
//                Id = IngredientSeeds.Water.Id
//            };
//            ingredient.Name += "updated";
//            ingredient.Description += "updated";

//            //Act
//            await _ingredientFacadeSUT.SaveAsync(ingredient);

//            //Assert
//            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
//            var ingredientFromDb = await dbxAssert.Ingredients.SingleAsync(i => i.Id == ingredient.Id);
//            DeepAssert.Equal(ingredient, Mapper.Map<IngredientDetailModel>(ingredientFromDb));
//        }
//    }
//}
