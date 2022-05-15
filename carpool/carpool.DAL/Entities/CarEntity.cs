using Carpool.Common.Enums;
namespace Carpool.DAL.Entities;

public record CarEntity(
    Guid Id,
    Manufacturer Manufacturer,
    CarType CarType,
    DateTime RegistrationDate,
    string? PhotoUrl,
    int SeatCapacity,
    Guid OwnerId ) : IEntity
{
#nullable disable
    public CarEntity() : this(default, default, default, default, string.Empty, default, default) { }
#nullable enable
    public UserEntity? Owner { get; init; }
    public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();

}
