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
        public UserEntity? Owner { get; init; }
    }

