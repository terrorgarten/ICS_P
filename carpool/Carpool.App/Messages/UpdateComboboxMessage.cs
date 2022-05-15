using Carpool.BL.Models;

namespace Carpool.App.Messages
{
    public record UpdateComboboxMessage<T> : Message<T>
        where T : IModel
    {
    }
}