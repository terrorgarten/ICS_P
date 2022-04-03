using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Carpool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;


namespace Carpool.Common.Tests.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity EmptyRideEntity = new(
        Id: default,
        Start: default!,
        End: default!,
        BeginTime: default,
        ApproxRideTime: default,
        UserId: default,
        CarId: default
    )
    {
        User = default,
        PassengerRides = default,
        Car = default
    };

    public static readonly RideEntity RideEntity = new(
        Id: Guid.Parse(input: "7953ACF7-1CBD-410A-835F-BA332A591164"),
        Start: "Praha",
        End: "Brno",
        BeginTime: new DateTime(2020, 1, 1, 19, 30, 0),
        ApproxRideTime: TimeSpan.FromHours(value: 2),
        UserId: UserSeeds.UserEntity.Id, //Franta
        CarId: CarSeeds.SportCar.Id 
    )
    {
        //?? nesedí mi tu ještě jedna entitka z userrides
    };

    public static readonly RideEntity RideEntity1 = new(
        Id: Guid.Parse(input: "2D87B86E-AD98-4435-A9D2-EAEF5B9BA0BB"),
        Start: "Olomouc",
        End: "Ostrava",
        BeginTime: new DateTime(2019, 6, 4, 9, 0, 0),
        ApproxRideTime: TimeSpan.FromHours(value: 1),
        UserId: UserSeeds.UserEntity1.Id,
        CarId: CarSeeds.CarEntity1.Id
    )
    {
        User = UserSeeds.UserEntity1.Id,
        PassengerRides = UserRideSeeds.UserRideEntity1 //?? celá entita nebo jen nějaký atribut
        Car = CarSeeds.CarEntity1
    };

    public static readonly RideEntity RideEntity2 = new(
        Id: Guid.Parse(input: "16E2290B-E1E0-4ADD-9BFD-6B3026AB2F3F"),
        Start: "Hradec Králové",
        End: "Brno",
        BeginTime: new DateTime(2019, 6, 15, 10, 50, 0),
        ApproxRideTime: TimeSpan.FromHours(value: 2.0),
        UserId: UserSeeds.UserEntity2.Id,
        CarId: CarSeeds.CarEntity2.Id
    )
    {
        User = UserSeeds.UserEntity2.Id,
        PassengerRides = UserRideSeeds.UserRideEntity2 //?? viz výš
        Car = CarSeeds.CarEntity2
    };


    //To ensure that no tests reuse these clones for non-idempotent operations:
    public static readonly RideEntity RideEntityWithNoPassengers = RideEntity with
    {
        Id = Guid.Parse(input: "659E3788-5F96-4287-AB86-395C8A5AE9BC"),
        User = null,
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityUpdate = RideEntity with
    {
        Id = Guid.Parse(input: "224B3DF0-0284-4827-B92D-DDFE245B907B"),
        User = null,
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityDelete = RideEntity with
    {
        Id = Guid.Parse(input: "E89BEEAE-CE1C-4662-95DE-1DFB99D7E6F0"),
        User = null,
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityForRideUserUpdate = RideEntity with
    {
        Id = Guid.Parse(input: "85EFC35D-5F7C-490F-80C9-5789D5AB076E"),
        User = null,
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };

    public static readonly RideEntity RideEntityForRideUserDelete = RideEntity with
    {
        Id = Guid.Parse(input: "919959EC-2251-44B4-AF3E-8E0858AE5D80"),
        User = null,
        PassengerRides = Array.Empty<UserRideEntity>(),
        Car = null
    };
    


    static RideSeeds()
    {
        RideEntity.PassengerRides.Add(UserRideSeeds.UserRideEntity1);
        RideEntity.PassengerRides.Add(UserRideSeeds.UserRideEntity2);
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            RideEntity with
            {
                User = null,
                PassengerRides = Array.Empty<UserRideEntity>(),
                Car = null
            },
            RideEntityWithNoPassengers,
            RideEntityUpdate,
            RideEntityDelete,
            RideEntityForRideUserUpdate,
            RideEntityForRideUserDelete
        );
    }
}


