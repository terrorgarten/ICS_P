using carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace carpool.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity Ride1 = new(
        Id: Guid.NewGuid(),
        Start: "Brno",
        End: "Praha",
        BeginTime: new DateTime(2022, 10, 28, 10, 30, 0),
        ApproxRideTime: TimeSpan.FromHours(2.5),
        CarId: CarSeeds.BigCar.Id,
        UserId: UserSeeds.FirstUser.Id)
    { 
        Car = CarSeeds.BigCar,
        User = UserSeeds.FirstUser
    };

    //static RideSeeds()
    //{

    //}

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(Ride1 with { Car = null, User = null });
    }
}