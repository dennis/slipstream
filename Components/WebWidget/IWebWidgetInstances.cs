#nullable enable


using System.Collections.Generic;

namespace Slipstream.Components.WebWidget
{
    public interface IWebWidgetInstances
    {
        void Add(string id, string type, string? data);
        void Remove(string id);
        string this[string id] {get;}
        ICollection<string> GetIds();
        string? InitData(string id);
    }
}