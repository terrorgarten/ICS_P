namespace carpool.DAL.Entities;

public record UserRideEntity
{
    public Guid Id { get; set; }
    public Guid PassangerId { get; set; }
    public UserEntity Passenger { get; set; }
    public Guid RideId { get; set; }
    public RideEntity Ride { get; set; }
}