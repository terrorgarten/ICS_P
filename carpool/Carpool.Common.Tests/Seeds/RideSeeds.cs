﻿using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Common.Tests.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity EmptyRideEntity = new(
        default,
        default!,
        default!,
        default,
        default,
        default
    )
    {
        Car = default
    };

    public static readonly RideEntity RideEntity = new(
        Guid.Parse("7953ACF7-1CBD-410A-835F-BA332A591164"),
        "Praha",
        "Brno",
        new DateTime(2020, 1, 1, 19, 30, 0),
        TimeSpan.FromHours(2),
        CarSeeds.SportCar.Id
    )
    {
        Car = CarSeeds.SportCar
    };

    public static readonly RideEntity RideEntityForUserRideEntity = new(
        Guid.Parse("2D87B86E-AD98-4435-A9D2-EAEF5B9BA0BB"),
        "Olomouc",
        "Ostrava",
        new DateTime(2019, 6, 4, 9, 0, 0),
        TimeSpan.FromHours(1),
        CarSeeds.CarEntity1.Id
    )
    {
        Car = CarSeeds.CarEntity1
    };


    //To ensure that no tests reuse these clones for non-idempotent operations:
    public static readonly RideEntity RideEntityWithNoPassengers = RideEntity with
    {
        Id = Guid.Parse("659E3788-5F96-4287-AB86-395C8A5AE9BC"),
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityForRideTestsGet = RideEntity with
    {
        Id = Guid.Parse("16E2290B-E1E0-4ADD-9BFD-6B3026AB2F3F"),
        //PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityUpdate = RideEntity with
    {
        Id = Guid.Parse("224B3DF0-0284-4827-B92D-DDFE245B907B"),
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityDelete = RideEntity with
    {
        Id = Guid.Parse("E89BEEAE-CE1C-4662-95DE-1DFB99D7E6F0"),
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };


    public static readonly RideEntity RideEntityForRideUserDelete = RideEntity with
    {
        Id = Guid.Parse("919959EC-2251-44B4-AF3E-8E0858AE5D80"),
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };


    static RideSeeds()
    {
        RideEntityForUserRideEntity.PassengerRides.Add(UserRideSeeds.UserRideEntity1);
        RideEntityForRideTestsGet.PassengerRides.Add(UserRideSeeds.UserRideEntity2);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            RideEntity with
            {
                PassengerRides = Array.Empty<UserRideEntity>(),
                Car = null
            },
            RideEntityForRideTestsGet with
            {
                PassengerRides = Array.Empty<UserRideEntity>(),
                Car = null
            },
            RideEntityWithNoPassengers,
            RideEntityUpdate,
            RideEntityDelete,
            RideEntityForUserRideEntity with
            {
                PassengerRides = Array.Empty<UserRideEntity>(),
                Car = null
            },
            RideEntityForRideUserDelete
        );
    }
}