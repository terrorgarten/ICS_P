namespace Carpool.DAL.Entities;

public record RideEntity(
    Guid Id,
    string Start,
    string End,
    DateTime BeginTime,
    TimeSpan ApproxRideTime,
    Guid UserId,
    Guid CarId) : IEntity
{
    public UserEntity? User { get; init; }
    public ICollection<UserRideEntity> PassengerRides { get; init; } = new List<UserRideEntity>();
    public CarEntity? Car { get; init; }
}