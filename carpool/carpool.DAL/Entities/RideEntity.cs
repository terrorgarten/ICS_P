using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace carpool.DAL.Entities;
//Start(místo, poloha)
//Cíl(místo, poloha)
//Čas začátku
//Předpokládaný čas konce, nebo předpokládaná doba cesty
//(Řidič)
//(Spolujezdci)
//(Automobil)
public record RideEntity(
    Guid Id,
    string Start,
    string End,
    DateTime BeginTime,
    TimeSpan ApproxRideTime,
    Guid UserId,
    Guid CarId) : IEntity
{
    public UserEntity? User { get; set; }
    public ICollection<UserRideEntity> PassengerRides { get; set; }// = new List<UserRideEntity>();
    public CarEntity? Car { get; init; }
}