using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity FirstUser = new(
        Id: Guid.NewGuid(),
        Name: "Jožko",
        Surname: "Mrkvička",
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/200-2004145_selfie-png-transparent-png.png");

    static UserSeeds()
    {
        FirstUser.OwnedCars.Add(CarSeeds.BigCar);
        //FirstUser.DriverRides.Add(RideSeeds.Ride1);
        //FirstUser.PassangerRides.Add(RideSeeds.Ride2);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            FirstUser with{ OwnedCars = Array.Empty<CarEntity>() });
    }
}