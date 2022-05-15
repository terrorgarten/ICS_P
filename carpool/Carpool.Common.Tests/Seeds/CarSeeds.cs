using Carpool.Common.Enums;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Seeds;

public static class CarSeeds
{
    public static readonly CarEntity EmptyCarEntity = new(
        default,
        default,
        default,
        default,
        default!,
        default,
        default)
    {
        Owner = default!
    };


    public static readonly CarEntity SportCar = new(
        Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        Manufacturer.BMW,
        CarType.Cabriolet,
        new DateTime(2015, 10, 1),
        null,
        2,
        UserSeeds.UserEntity.Id)
    {
        Owner = UserSeeds.UserEntity
    };


    public static readonly CarEntity CarEntity1 = new(
        Guid.Parse("df935095-8709-4040-a2bb-b6f97cb416dc"),
        Manufacturer.Kia,
        CarType.Crossover,
        new DateTime(2016, 9, 1),
        null,
        4,
        UserSeeds.UserEntity1.Id)
    {
        Owner = UserSeeds.UserEntity1
    };

    public static readonly CarEntity CarEntity2 = new(
        Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
        Manufacturer.Skoda,
        CarType.Sedan,
        new DateTime(2020, 2, 28),
        null,
        4,
        UserSeeds.UserEntity2.Id)
    {
        Owner = UserSeeds.UserEntity2
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly CarEntity CarEntityUpdate =
        SportCar with
        {
            Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052"), Owner = null,
            OwnerId = UserSeeds.UserEntityUpdate.Id
        };

    public static readonly CarEntity CarEntityDelete =
        SportCar with {Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619"), Owner = null};

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarEntity>().HasData(
            CarEntity1 with {Owner = null},
            CarEntity2 with {Owner = null},
            SportCar with {Owner = null},
            CarEntityUpdate,
            CarEntityDelete);
    }
}