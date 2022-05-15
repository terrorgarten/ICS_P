using Carpool.Common.Enums;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.DAL.Seeds;

public static class CarSeeds
{
    public static readonly CarEntity SportCar = new(
        Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        Manufacturer.BMW,
        CarType.Cabriolet,
        new DateTime(2015, 10, 1),
        @"https://images.pexels.com/photos/170811/pexels-photo-170811.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=650&w=940",
        2,
        UserSeeds.FirstUser.Id)
    {
        Owner = UserSeeds.FirstUser
    };

    public static readonly CarEntity PersonalCar = new(
        Guid.Parse("3D08D1F3-7499-4899-B828-BCBA6C08A5C6"),
        Manufacturer.Fiat,
        CarType.Universal,
        new DateTime(2016, 5, 28),
        @"https://images.pexels.com/photos/7459482/pexels-photo-7459482.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=650&w=940",
        4,
        UserSeeds.SecondUser.Id)
    {
        Owner = UserSeeds.SecondUser
    };

    public static readonly CarEntity BatMobile = new(
        Guid.Parse("4BFBD813-0CD6-4D66-8F62-840503E597B4"),
        Manufacturer.Audi,
        CarType.Convertible,
        new DateTime(2017, 6, 28),
        @"https://images.pexels.com/photos/1035108/pexels-photo-1035108.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=650&w=940",
        4,
        UserSeeds.SecondUser.Id)
    {
        Owner = UserSeeds.SecondUser
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarEntity>().HasData(
            SportCar with {Owner = null},
            PersonalCar with {Owner = null},
            BatMobile with {Owner = null});
    }
}