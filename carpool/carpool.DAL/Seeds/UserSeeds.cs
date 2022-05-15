using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity FirstUser = new(
        Guid.Parse("E8C82085-4195-4C1E-BFFB-D6A4069BF997"),
        "Jožko",
        "Mrkvička",
        @"https://images.pexels.com/photos/1933873/pexels-photo-1933873.jpeg?auto=compress&cs=tinysrgb&dpr=3&h=750&w=1260");

    public static readonly UserEntity SecondUser = new(
        Guid.Parse("050FBEEB-7DC6-494B-A5AD-485B129CC4E6"),
        "Franta",
        "Bobek",
        @"https://images.pexels.com/photos/220453/pexels-photo-220453.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=650&w=940");

    static UserSeeds()
    {
        FirstUser.OwnedCars!.Add(CarSeeds.SportCar);
        FirstUser.PassengerRides!.Add(UserRideSeeds.UserRide2);

        SecondUser.OwnedCars!.Add(CarSeeds.PersonalCar);
        SecondUser.PassengerRides!.Add(UserRideSeeds.UserRide1);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            FirstUser with { OwnedCars = Array.Empty<CarEntity>(), PassengerRides = Array.Empty<UserRideEntity>() },
            SecondUser with { OwnedCars = Array.Empty<CarEntity>(), PassengerRides = Array.Empty<UserRideEntity>() }
        );
    }
}