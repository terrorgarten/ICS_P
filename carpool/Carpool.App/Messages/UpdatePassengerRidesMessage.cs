using Carpool.BL.Models;

namespace Carpool.App.Messages
{
    public record UpdatePassengerRidesMessage<T> : Message<T>
        where T : IModel
    {
    }
}