using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services
{
    public interface IEventSerdeService
    {
        IEvent? Deserialize(string json);
        string Serialize(IEvent @event);
    }
}
