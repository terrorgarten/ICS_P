﻿
namespace carpool.DAL.Entities;
    //Jméno
    //Příjmení 
    //Fotografie
    //(Vlastněná auta)
    //(Spolujízdy z pohledu řidiče)
    //(Spolujízdy z pohledu spolujezdce)
    public record UserEntity(
        Guid Id,
        string Name,
        string Surname,
        string? PhotoUrl) : IEntity
    {
        public ICollection<CarEntity> OwnedCars { get; init; } = new List<CarEntity>();
        public ICollection<UserRideEntity> PassengerRides { get; init; } = new List<UserRideEntity>(); 
        public ICollection<RideEntity> DriverRides { get; init; } = new List<RideEntity>();         
    }
