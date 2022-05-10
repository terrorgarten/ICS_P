using Carpool.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using Carpool.BL.Facades;
using Carpool.Common.Tests;
using Carpool.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Carpool.BL.Tests
{
    public sealed class UserRideFacadeTests : CRUDFacadeTestsBase
    {
        
        private readonly UserRideFacade _userRideFacadeSUT;

        public UserRideFacadeTests(ITestOutputHelper output) : base(output)
        {
            _userRideFacadeSUT = new UserRideFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task GetUserRide()
        {

            var filtered = await _userRideFacadeSUT.GetUserRides
                (UserSeeds.UserForUserRideEntity.Id);
            
        }
        
        [Fact]
        public async Task Create_WithNonExistingItem_UserRideDetail_DoesNotThrow()
        {
            var user = new UserRideDetailModel
            (
                Name: UserSeeds.UserEntity1.Name,
                Surname: UserSeeds.UserEntity1.Surname

            )
            {
                RideId = RideSeeds.RideEntity.Id,
                PassengerId = UserSeeds.UserEntity1.Id
            };
            


            var _ = await _userRideFacadeSUT.SaveCheckAsync(UserSeeds.UserEntity1.Id, RideSeeds.RideEntity.Id );
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
                    Name: UserSeeds.UserEntity1.Name,
                    Surname: UserSeeds.UserEntity1.Surname
            )
            {
                RideId = RideSeeds.RideEntity.Id,
                PassengerId = UserSeeds.UserEntity1.Id
            };
            var _ = await _userRideFacadeSUT.SaveAsync(seeded_user);

            var user = await _userRideFacadeSUT.GetAsync(UserRideSeeds.UserRideEntity1.Id);
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
        

        //[Fact]
        //public async Task NewUserRide_Insert_UserRideAdded()
        //{
        //    //Arrange
        //    var user = new UserRideDetailModel
        //    (
        //        Name: UserSeeds.UserForUserRideEntity.Name,
        //        Surname: UserSeeds.UserForUserRideEntity.Surname
        //    )
        //    {
        //        RideId = RideSeeds.RideEntityForUserRideEntity.Id,
        //        PassengerId = UserSeeds.UserForUserRideEntity.Id
        //    };

        //    //Act
        //    user = await _userRideFacadeSUT.SaveAsync(user);

        //    //Assert
        //    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        //    var userFromDb = await dbxAssert.UsersRideEntity.SingleAsync(i => i.Id == user.Id);
        //    DeepAssert.Equal(user, Mapper.Map<UserRideDetailModel>(userFromDb));
        //}
        
        //[Fact]
        //public async Task SeededUserRideEntity_Update_UserRideUpdated()
        //{
        //    //Arrange
        //    var user = new UserRideDetailModel
        //    (
        //        Name: UserSeeds.UserForUserRideEntity.Name,
        //        Surname: UserSeeds.UserForUserRideEntity.Surname
        //    )
        //    {
        //        RideId = RideSeeds.RideEntityForUserRideEntity.Id,
        //        PassengerId = UserSeeds.UserForUserRideEntity.Id
        //    };
        //    user.Name += "updated";
        //    user.Surname += "updated";

        //    //Act
        //    await _userRideFacadeSUT.SaveAsync(user);

        //    var updated_user = await _userRideFacadeSUT.GetAsync(UserRideSeeds.UserRideEntity1.Id);
        //    //Assert
        //    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        //    var userFromDb = await dbxAssert.UsersRideEntity.SingleAsync(i => i.Id == user.Id);
        //    DeepAssert.Equal(user, Mapper.Map<UserRideDetailModel>(userFromDb));
        //}
    }
}
