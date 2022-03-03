using carpool.common;
using carpool.common.Enums;

namespace carpool.DAL.Entities;

    //Výrobce
    //Typ
    //Datum první registrace
    //Fotografie
    //Počet míst k sezení
    //(Majitel, tj.uživatel)
    public record CarEntity(
            Guid Id,
            Manufacturer Manufacturer,
            CarType CarType,
            DateOnly RegistrationDate,
            string? PhotoUrl,
            int SeatCapacity,
            Guid OwnerId ) : IEntity
    {
    //#nullable disable
    //    public CarEntity() : this(default, default, default, default, default, default, default) { }
    //#nullable enable
        public UserEntity? Owner { get; init; } //idk
    }

