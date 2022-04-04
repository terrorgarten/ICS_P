//using System.Linq;
//using System.Threading.Tasks;
//using Carpool.DAL;
//using Carpool.DAL.Entities;
//using Carpool.DAL.Seeds;
//using Microsoft.EntityFrameworkCore;
//using Xunit;
//using Xunit.Abstractions;

//namespace Carpool.DAL.Tests
//{
//    public class DbContextCarTests : DbContextTestsBase
//    {
//        public DbContextCarTests(ITestOutputHelper output) : base(output)
//        {
//        }

//        [Fact]
//        public async Task GetAll_Cars_CheckIDs()
//        {
//            var cars = await CarpoolDbContextSUT.Cars
//                        //.Where(i => i.Id == cars.)
//                        .ToArrayAsync();


//            Assert.Contains(CarSeeds.SportCar with { Owner = null}, cars);
//            Assert.Contains(CarSeeds.PersonalCar with { Owner = null }, cars);
//        }

//        [Fact]
//        public async Task GetAll_Cars_SeededBigCarExists()
//        {
//            var cars
//                = await CarpoolDbContextSUT
//                    .Cars
//                    .FirstOrDefaultAsync(i => i.Id == CarSeeds.SportCar.Id);
//            Assert.NotNull(cars);
//            Assert.Equal(CarSeeds.SportCar with { Owner = null }, cars);
//        }

//        [Fact]
//        public async Task GetAll_Rides_SeededRide1Exists()
//        {
//            var rides
//                = await CarpoolDbContextSUT
//                    .Rides
//                    .FirstOrDefaultAsync(i => i.Id == RideSeeds.Ride1.Id);
//            Assert.NotNull(rides);
//            Assert.Equal(RideSeeds.Ride1.Id, rides.Id);
//        }

//    }
//}