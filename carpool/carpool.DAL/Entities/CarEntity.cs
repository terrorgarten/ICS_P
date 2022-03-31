using carpool.common.Enums;

namespace carpool.DAL.Entities;

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

