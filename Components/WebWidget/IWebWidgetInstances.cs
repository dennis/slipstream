#nullable enable

using System.Collections.Generic;

using Slipstream.Shared;

namespace Slipstream.Components.WebWidget
{
    public interface IWebWidgetInstances
    {
        int Count { get; }

        public class Instance
        {
            public string InstanceId { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string? InitData { get; set; }
            public IEventEnvelope Envelope { get; set; }

            public Instance(string instanceId)
            {
                Envelope = new EventEnvelope(instanceId);
            }

            public Instance Clone()
            {
                return new Instance(Envelope.Sender) { Type = Type, InstanceId = InstanceId, InitData = InitData };
            }
        }

        void Add(string id, string type, string? data);

        void Remove(string id);

        Instance this[string id] { get; }

        bool TryGetValue(string instanceId, out Instance result);

        ICollection<string> GetIds();
    }
}