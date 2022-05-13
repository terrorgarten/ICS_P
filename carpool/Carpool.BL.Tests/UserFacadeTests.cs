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
    public sealed class UserFacadeTests : CRUDFacadeTestsBase
    {
        private readonly UserFacade _userFacadeSUT;

        public UserFacadeTests(ITestOutputHelper output) : base(output)
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithNonExistingItem_UserDetail_DoesNotThrow()
        {
            var user = new UserDetailModel
            (
                Name: "Danoch",
                Surname: "Udy",
                PhotoUrl: null
            );
            var _ = await _userFacadeSUT.SaveAsync(user);
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
            await _userFacadeSUT.DeleteAsync(UserSeeds.UserEntityDelete.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserSeeds.UserEntityDelete.Id));
        }


        [Fact]
        public async Task NewUser_InsertOrUpdate_UserAdded()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.UserEntity2.Name,
                Surname: UserSeeds.UserEntity2.Surname,
                PhotoUrl: UserSeeds.UserEntity2.PhotoUrl
            )
            {
                Id = UserSeeds.UserEntity2.Id
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
                Name: UserSeeds.UserEntity2.Name,
                Surname: UserSeeds.UserEntity2.Surname,
                PhotoUrl: UserSeeds.UserEntity2.PhotoUrl
            )
            {
                Id = UserSeeds.UserEntity2.Id
            };
            user.Name += "updated";
            user.Surname += "updated";

            //Act
            await _userFacadeSUT.SaveAsync(user);

            var updated_user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity2.Id);
            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(updated_user, Mapper.Map<UserDetailModel>(userFromDb));
        }
    }
}
