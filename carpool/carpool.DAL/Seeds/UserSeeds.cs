using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity FirstUser = new(
        Id: Guid.Parse(input: "E8C82085-4195-4C1E-BFFB-D6A4069BF997"),
        Name: "Jožko",
        Surname: "Mrkvička",
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/200-2004145_selfie-png-transparent-png.png");

    public static readonly UserEntity SecondUser = new(
        Id: Guid.Parse(input: "050FBEEB-7DC6-494B-A5AD-485B129CC4E6"),
        Name: "Franta",
        Surname: "Bobek",
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/346-3468375_spiderman-mcu-ps4-selfie-hd-png-download.png");

    static UserSeeds()
    {
        FirstUser.OwnedCars.Add(CarSeeds.SportCar);
        SecondUser.OwnedCars.Add(CarSeeds.PersonalCar);
        FirstUser.DriverRides.Add(RideSeeds.Ride1);
        SecondUser.PassengerRides.Add(UserRideSeeds.UserRide1);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            FirstUser with{ OwnedCars = Array.Empty<CarEntity>(), DriverRides = Array.Empty<RideEntity>(), PassengerRides = Array.Empty<UserRideEntity>() },
            SecondUser with { OwnedCars = Array.Empty<CarEntity>(), DriverRides = Array.Empty<RideEntity>(), PassengerRides = Array.Empty<UserRideEntity>() }
        );
    }
}