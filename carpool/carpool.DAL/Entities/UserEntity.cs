namespace Carpool.DAL.Entities;

public record UserEntity(
    Guid Id,
    string Name,
    string Surname,
    string? PhotoUrl) : IEntity
{
#nullable disable
    public UserEntity() : this(default, string.Empty, string.Empty, string.Empty) { }
#nullable enable
    public ICollection<CarEntity>? OwnedCars { get; init; } = new List<CarEntity>();
    public ICollection<UserRideEntity>? PassengerRides { get; init; } = new List<UserRideEntity>();
}
