#nullable enable

using System.Collections.Generic;

using static Slipstream.Components.WebWidget.IWebWidgetInstances;

namespace Slipstream.Components.WebWidget
{
    public class WebWidgetInstances : IWebWidgetInstances
    {
        private readonly IDictionary<string, Instance> Instances = new Dictionary<string, Instance>();

        public int Count
        {
            get
            {
                lock (Instances)
                    return Instances.Count;
            }
        }

        public void Add(string instanceId, string webwidgetType, string? initData)
        {
            lock (Instances)
            {
                Instances.Add(instanceId, new Instance(instanceId) { InstanceId = instanceId, Type = webwidgetType, InitData = initData });
            }
        }

        public void Remove(string instanceId)
        {
            lock (Instances)
            {
                Instances.Remove(instanceId);
            }
        }

        public bool TryGetValue(string instanceId, out Instance result)
        {
            if (Instances.TryGetValue(instanceId, out Instance? obj))
            {
                result = obj;
                return true;
            }
            else
            {
                result = new Instance("UNKNOWN");
                return false;
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

        public bool Contains(string id)
        {
            lock (Instances)
                return Instances.ContainsKey(id);
        }
    }
}