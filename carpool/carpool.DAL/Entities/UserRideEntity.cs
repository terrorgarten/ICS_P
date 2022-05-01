namespace Carpool.DAL.Entities;

public record UserRideEntity (
    Guid Id,
    Guid PassengerId,
    Guid RideId) : IEntity
{
#nullable disable
    public UserRideEntity() : this(default, default, default) { }
#nullable enable
    public UserEntity? Passenger { get; init; }
    public RideEntity? Ride { get; init; }
}