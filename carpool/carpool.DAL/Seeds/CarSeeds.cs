using Carpool.Common.Enums;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carpool.DAL.Seeds;

public static class CarSeeds
{
    public static readonly CarEntity SportCar = new(
        Id: Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        Manufacturer: Manufacturer.BMW,
        CarType: CarType.Cabriolet,
        RegistrationDate: DateOnly.Parse("01/05/2015"),
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/9-99883_beige-bmw-sedan-5-2013-car-png-clipart.png",
        SeatCapacity: 4,
        OwnerId: UserSeeds.FirstUser.Id)
    {
        Owner = UserSeeds.FirstUser
    };
 
    public static readonly CarEntity PersonalCar = new(
        Id: Guid.Parse(input: "3D08D1F3-7499-4899-B828-BCBA6C08A5C6"),
        Manufacturer: Manufacturer.Fiat,
        CarType: CarType.Universal,
        RegistrationDate: DateOnly.Parse("01/06/2015"),
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/79-793999_land-vehicle-city-car-red-fiat-500-rim.png",
        SeatCapacity: 4,
        OwnerId: UserSeeds.SecondUser.Id)
    {
        Owner = UserSeeds.SecondUser
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarEntity>().HasData(
            SportCar with { Owner = null },
            PersonalCar with { Owner = null });
    }
}