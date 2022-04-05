using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUserEntity = new(
        Id: default,
        Name: default!,//stringy musia mat !
        Surname: default!,
        PhotoUrl: default);


    public static readonly UserEntity UserEntity = new(
        Id: Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        Name: "Jožko",
        Surname: "Mrkvička",
        PhotoUrl: null);

    public static readonly UserEntity UserForUserRideEntity = new(
        Id: Guid.Parse(input: "3D8369CC-263E-42D3-AF62-010BAEEF7D3C"),
        Name: "Bob",
        Surname: "Bobek",
        PhotoUrl: null);
    
    public static readonly UserEntity UserEntity1 = new(
        Id: Guid.Parse(input: "2FFB085E-6E24-4D42-BBE9-8F7EB2581B49"),
        Name: "Franta",
        Surname: "Barda",
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/200-2004145_selfie-png-transparent-png.png");

    public static readonly UserEntity UserEntity2 = new(
        Id: Guid.Parse(input: "CD97DFE2-CC2A-4215-BA52-4D7D02A35542"),
        Name: "Bozena",
        Surname: "Hlavacova",
        PhotoUrl: @"https://png.pngitem.com/pimgs/s/200-2003995_transparent-pregnant-woman-png-png-download.png");

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly UserEntity UserEntityWithNoPassengerRides = UserEntity with
    {
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"),
        OwnedCars = Array.Empty<CarEntity>(),
        PassengerRides = Array.Empty<UserRideEntity>(),
        DriverRides = Array.Empty<RideEntity>()
    };
    public static readonly UserEntity UserEntityUpdate = UserEntity with { 
        Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"),
        OwnedCars = Array.Empty<CarEntity>(),
        PassengerRides = Array.Empty<UserRideEntity>(),
        DriverRides = Array.Empty<RideEntity>()
    };
    public static readonly UserEntity UserEntityDelete = UserEntity with { 
        Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"),
        OwnedCars = Array.Empty<CarEntity>(),
        PassengerRides = Array.Empty<UserRideEntity>(),
        DriverRides = Array.Empty<RideEntity>()
    };

    public static readonly UserEntity UserForUserRideEntityUpdate = UserEntity with { 
        Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"),
        OwnedCars = Array.Empty<CarEntity>(),
        PassengerRides = Array.Empty<UserRideEntity>(),
        DriverRides = Array.Empty<RideEntity>()
    };
    public static readonly UserEntity UserForUserRideEntityDelete = UserEntity with { 
        Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"),
        OwnedCars = Array.Empty<CarEntity>(),
        PassengerRides = Array.Empty<UserRideEntity>(),
        DriverRides = Array.Empty<RideEntity>()
    };

    static UserSeeds()
    {
        UserForUserRideEntity.PassengerRides.Add(UserRideSeeds.UserRideEntity1);
        //UserEntity2.PassengerRides.Add(UserRideSeeds.UserRideEntity2);
        //UserEntity1.OwnedCars.Add(CarSeeds.CarEntity1);
        UserEntity2.OwnedCars.Add(CarSeeds.CarEntity2);
        UserEntity.OwnedCars.Add(CarSeeds.SportCar);
        UserEntity1.OwnedCars.Add(CarSeeds.CarEntity1);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            UserEntity with {
                OwnedCars = Array.Empty<CarEntity>(),
                PassengerRides = Array.Empty<UserRideEntity>(),
                DriverRides = Array.Empty<RideEntity>()
            },
            UserForUserRideEntity with
            {
                OwnedCars = Array.Empty<CarEntity>(),
                PassengerRides = Array.Empty<UserRideEntity>(),
                DriverRides = Array.Empty<RideEntity>()
            },
            UserEntity1 with
            {
                OwnedCars = Array.Empty<CarEntity>(),
                PassengerRides = Array.Empty<UserRideEntity>(),
                DriverRides = Array.Empty<RideEntity>()
            },
            UserEntity2 with
            {
                OwnedCars = Array.Empty<CarEntity>(),
                PassengerRides = Array.Empty<UserRideEntity>(),
                DriverRides = Array.Empty<RideEntity>()
            },
            UserEntityWithNoPassengerRides,
            UserEntityUpdate,
            UserEntityDelete,
            UserForUserRideEntityUpdate,
            UserForUserRideEntityDelete
        );
    }
}