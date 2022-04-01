namespace Carpool.DAL.Entities;

public record UserRideEntity (
    Guid Id,
    Guid PassangerId,
    Guid RideId) : IEntity
{
    public UserEntity? Passenger { get; init; }
    public RideEntity? Ride { get; init; }
}