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
                (RideSeeds.RideEntity.Id);


        }
        /*
        [Fact]
        public async Task Create_WithNonExistingItem_UserRideDetail_DoesNotThrow()
        {
            var user = new UserRideDetailModel
            (
                Name: "Danoch",
                Surname: "Udy"
            );
            var _ = await _userRideFacadeSUT.SaveAsync(user);
        }

        
        [Fact]
        public async Task GetAll_Single_SeededUser()
        {
            var users = await _userFacadeSUT.GetAsync();
            var user = users.Single(i => i.Id == UserSeeds.UserEntity.Id);

            DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeeds.UserEntity), user);
        }
        
        [Fact]
        public async Task Insert_SeededUser()
        {
            var seeded_user = new UserDetailModel(
                    Name: UserSeeds.UserEntity1.Name,
                    Surname: UserSeeds.UserEntity1.Surname,
                    PhotoUrl: null
                );
            var _ = await _userFacadeSUT.SaveAsync(seeded_user);

            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
        }

        [Fact]
        public async Task GetById_SeededUser()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);

            DeepAssert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.UserEntity1), user);
        }

        [Fact]
        public async Task GetById_NonExistentUser()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.EmptyUserEntity.Id);

            Assert.Null(user);
        }

        [Fact]
        public async Task SeededUser_DeleteById_Deleted()
        {
            await _userFacadeSUT.DeleteAsync(UserSeeds.UserEntity1.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserSeeds.UserEntity1.Id));
        }


        [Fact]
        public async Task NewUser_InsertOrUpdate_UserAdded()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.UserEntity1.Name,
                Surname: UserSeeds.UserEntity1.Surname,
                PhotoUrl: UserSeeds.UserEntity1.PhotoUrl
            )
            {
                Id = UserSeeds.UserEntity1.Id
            };

            //Act
            user = await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }

        [Fact]
        public async Task SeededUserEntity_InsertOrUpdate_UserUpdated()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.UserEntity.Name,
                Surname: UserSeeds.UserEntity.Surname,
                PhotoUrl: UserSeeds.UserEntity.PhotoUrl
            )
            {
                Id = UserSeeds.UserEntity.Id
            };
            user.Name += "updated";
            user.Surname += "updated";

            //Act
            await _userFacadeSUT.SaveAsync(user);

            var updated_user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity.Id);
            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }*/
    }
}
