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
            var user = users.Single(i => i.Id == UserSeeds.FirstUser.Id);

            DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeeds.FirstUser), user);
        }


        [Fact]
        public async Task GetById_SeededUser()
        {
            var user= await _userFacadeSUT.GetAsync(UserSeeds.FirstUser.Id);

            DeepAssert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.FirstUser), user);
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
            await _userFacadeSUT.DeleteAsync(UserSeeds.FirstUser.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Users.AnyAsync(i => i.Id == UserSeeds.FirstUser.Id));
        }


        [Fact]
        public async Task NewUser_InsertOrUpdate_UserAdded()
        {
            //Arrange
            var user = new UserDetailModel(
                Name: "Martin",
                Surname: "Vano",
                PhotoUrl: null
            );

            //Act
            user = await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }

        [Fact]
        public async Task SeededFirstUser_InsertOrUpdate_UserUpdated()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.FirstUser.Name,
                Surname: UserSeeds.FirstUser.Surname,
                PhotoUrl: UserSeeds.FirstUser.PhotoUrl
            )
            {
                Id = UserSeeds.FirstUser.Id
            };
            user.Name += "updated";
            user.Surname += "updated";

            //Act
            await _userFacadeSUT.SaveAsync(user);

            var updated_user = await _userFacadeSUT.GetAsync(UserSeeds.FirstUser.Id);
            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }
    }
}
