using Carpool.BL.Models;

namespace Carpool.App.Messages;

public record ReloadMessage<T> : Message<T>
    where T : IModel
{
}