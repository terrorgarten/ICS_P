namespace Carpool.DAL.Entities;

public record RideEntity(
    Guid Id,
    string Start,
    string End,
    DateTime BeginTime,
    TimeSpan ApproxRideTime,
    Guid CarId) : IEntity
{
#nullable disable
    public RideEntity() : this(default, string.Empty, string.Empty, default, default, default)
    {
    }
#nullable enable
    public ICollection<UserRideEntity> PassengerRides { get; init; } = new List<UserRideEntity>();
    public CarEntity? Car { get; init; }
}