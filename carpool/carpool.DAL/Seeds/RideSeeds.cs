using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity Ride1 = new(
        Guid.Parse("91991AC5-2AB5-47B9-8356-F2A3B73C813F"),
        "Brno",
        "Praha",
        new DateTime(2022, 10, 28, 10, 30, 0),
        TimeSpan.FromHours(2.5),
        CarSeeds.SportCar.Id)
    {
        Car = CarSeeds.SportCar
    };

    public static readonly RideEntity Ride2 = new(
        Guid.Parse("85481279-3C93-4C97-B15C-40A5BD0EBB21"),
        "Brno",
        "Olomouc",
        new DateTime(2022, 09, 28, 10, 30, 0),
        TimeSpan.FromHours(2.5),
        CarSeeds.PersonalCar.Id)
    {
        Car = CarSeeds.PersonalCar
    };

    public static readonly RideEntity Ride3 = new(
        Guid.Parse("06DF6684-2100-4618-A72D-6185C6402249"),
        "Brno",
        "Bratislava",
        new DateTime(2022, 10, 28, 10, 30, 0),
        TimeSpan.FromHours(2.5),
        CarSeeds.SportCar.Id)
    {
        Car = CarSeeds.SportCar
    };

    public static readonly RideEntity Ride4 = new(
        Guid.Parse("65D5BC26-A782-4664-94CE-BCC348BB20F5"),
        "Zlín",
        "Olomouc",
        new DateTime(2022, 09, 28, 10, 30, 0),
        TimeSpan.FromHours(2.5),
        CarSeeds.PersonalCar.Id)
    {
        Car = CarSeeds.PersonalCar
    };

    static RideSeeds()
    {
        Ride1.PassengerRides.Add(UserRideSeeds.UserRide1);
        Ride2.PassengerRides.Add(UserRideSeeds.UserRide2);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            Ride1 with { Car = null, PassengerRides = Array.Empty<UserRideEntity>() },
            Ride2 with { Car = null, PassengerRides = Array.Empty<UserRideEntity>() },
            Ride3 with { Car = null, PassengerRides = Array.Empty<UserRideEntity>() },
            Ride4 with { Car = null, PassengerRides = Array.Empty<UserRideEntity>() });
    }
}