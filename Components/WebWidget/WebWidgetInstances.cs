#nullable enable

using System.Collections.Generic;

using static Slipstream.Components.WebWidget.IWebWidgetInstances;

namespace Slipstream.Components.WebWidget
{
    public class WebWidgetInstances : IWebWidgetInstances
    {
        private readonly IDictionary<string, Instance> Instances = new Dictionary<string, Instance>();

        public void Add(string instanceId, string webwidgetType, string? initData)
        {
            lock (Instances)
            {
                Instances.Add(instanceId, new Instance { InstanceId = instanceId, Type = webwidgetType, InitData = initData });
            }
        }

        public void Remove(string instanceId)
        {
            lock (Instances)
            {
                Instances.Remove(instanceId);
            }
        }

        public Instance this[string instanceId]
        {
            get
            {
                lock (Instances)
                {
                    return Instances[instanceId].Clone();
                }
            }
        }

        public ICollection<string> GetIds()
        {
            lock (Instances)
                return Instances.Keys;
        }
    }
}