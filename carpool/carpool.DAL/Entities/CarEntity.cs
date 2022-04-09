using System.ComponentModel.DataAnnotations;
using Carpool.Common.Enums;

namespace Carpool.DAL.Entities;

    public record CarEntity(
            Guid Id,
            Manufacturer Manufacturer,
            CarType CarType,
            //[DataType(DataType.Date)]
            DateTime RegistrationDate,
            string? PhotoUrl,
            int SeatCapacity,
            Guid OwnerId ) : IEntity
    {
#nullable disable
        public CarEntity() : this(default, default, default, default, default, default, default) { }
#nullable enable
    public UserEntity? Owner { get; init; }
    }

