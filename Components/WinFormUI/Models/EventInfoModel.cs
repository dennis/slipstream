using System;
using System.Collections.Generic;

namespace Slipstream.Components.WinFormUI.Models
{
    public class EventInfoModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Type EventType { get; set; } = typeof(Type); // This is expected to be overwritten!

        public IList<EventPropertyInfoModel> Properties { get; set; } = new List<EventPropertyInfoModel>();
    }
}