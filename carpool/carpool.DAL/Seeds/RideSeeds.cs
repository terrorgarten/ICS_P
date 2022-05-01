using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity Ride1 = new(
        Id: Guid.Parse(input: "91991AC5-2AB5-47B9-8356-F2A3B73C813F"),
        Start: "Brno",
        End: "Praha",
        BeginTime: new DateTime(2022, 10, 28, 10, 30, 0),
        ApproxRideTime: TimeSpan.FromHours(2.5),
        CarId: CarSeeds.SportCar.Id,
        UserId: UserSeeds.FirstUser.Id)
    {
        Car = CarSeeds.SportCar,
        User = UserSeeds.FirstUser
    };

    public static readonly RideEntity Ride2 = new(
        Id: Guid.Parse(input: "85481279-3C93-4C97-B15C-40A5BD0EBB21"),
        Start: "Brno",
        End: "Praha",
        BeginTime: new DateTime(2022, 10, 28, 10, 30, 0),
        ApproxRideTime: TimeSpan.FromHours(2.5),
        CarId: CarSeeds.PersonalCar.Id,
        UserId: UserSeeds.SecondUser.Id)
    {
        Car = CarSeeds.PersonalCar,
        User = UserSeeds.SecondUser
    };

    static RideSeeds()
    {
        Ride1.PassengerRides.Add(UserRideSeeds.UserRide1);
        Ride2.PassengerRides.Add(UserRideSeeds.UserRide2);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            Ride1 with { Car = null, User = null, PassengerRides = Array.Empty<UserRideEntity>() },
            Ride2 with { Car = null, User = null, PassengerRides = Array.Empty<UserRideEntity>() });
    }
}