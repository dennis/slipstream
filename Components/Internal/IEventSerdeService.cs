using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IEventSerdeService
    {
        IEvent? Deserialize(string json);

        string Serialize(IEvent @event);

        IEvent[] DeserializeMultiple(string json);

        string SerializeMultiple(IEvent[] events);
    }
}