#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.WebWidget
{
    public interface IWebWidgetInstances
    {
        public class Instance
        {
            public string InstanceId { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string? InitData { get; set; }

            public Instance Clone()
            {
                return new Instance { Type = Type, InstanceId = InstanceId, InitData = InitData };
            }
        }

        void Add(string id, string type, string? data);

        void Remove(string id);

        Instance this[string id] { get; }

        ICollection<string> GetIds();
    }
}