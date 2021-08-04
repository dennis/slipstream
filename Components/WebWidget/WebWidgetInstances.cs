#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.WebWidget
{
    public class WebWidgetInstances : IWebWidgetInstances
    {
        private readonly IDictionary<string, string> InstancesType = new Dictionary<string, string>();
        private readonly IDictionary<string, string?> InstanceInitData = new Dictionary<string, string?>();

        public void Add(string id, string type, string? data)
        {
            lock(InstancesType)
            {
                InstancesType.Add(id, type);
                InstanceInitData.Add(id, data);
            }
                
        }

        public void Remove(string id)
        {
            lock (InstancesType)
            {
                InstancesType.Remove(id);
                InstanceInitData.Remove(id);
            }
        }

        public string this[string id]
        {
            get
            {
                return InstancesType[id];
            }
        }

        public string? InitData(string id)
        {
            lock(InstanceInitData)
                if(InstanceInitData.ContainsKey(id))
                    return InstanceInitData[id];

            return null;
        }

        public ICollection<string> GetIds()
        {
            lock(InstancesType)
                return InstancesType.Keys;
        }
    }
}