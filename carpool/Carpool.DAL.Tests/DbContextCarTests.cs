using System.Linq;
using System.Threading.Tasks;
using Carpool.DAL;
using Carpool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Carpool.DAL.Tests
{
    public class DbContextCarTests : IAsyncLifetime
    {
        private readonly CarpoolDbContext _carpoolDbContextSut;

        public DbContextCarTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<CarpoolDbContext>();
            dbContextOptions.UseInMemoryDatabase("Carpool");
            _carpoolDbContextSut = new CarpoolDbContext(dbContextOptions.Options);
        }

        [Fact]
        public async Task GetAll_Users_SeededFirstUserExists()
        {
            var users
                = await _carpoolDbContextSut
                    .Users
                    .FirstOrDefaultAsync(i => i.Id == UserSeeds.FirstUser.Id);
            Assert.NotNull(users);
            Assert.Equal(UserSeeds.FirstUser.Id, users.Id);
        }

        [Fact]
        public async Task GetAll_Cars_SeededBigCarExists()
        {
            var cars
                = await _carpoolDbContextSut
                    .Cars
                    .FirstOrDefaultAsync(i => i.Id == CarSeeds.SportCar.Id);
            Assert.NotNull(cars);
            Assert.Equal(CarSeeds.SportCar with { Owner = null }, cars);
        }

        [Fact]
        public async Task GetAll_Rides_SeededRide1Exists()
        {
            var rides
                = await _carpoolDbContextSut
                    .Rides
                    .FirstOrDefaultAsync(i => i.Id == RideSeeds.Ride1.Id);
            Assert.NotNull(rides);
            Assert.Equal(RideSeeds.Ride1.Id, rides.Id);
        }

        public async Task InitializeAsync()
            => await _carpoolDbContextSut.Database.EnsureCreatedAsync();
       

        public async Task DisposeAsync()
            => await _carpoolDbContextSut.DisposeAsync();
    }
}