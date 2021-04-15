using System;
using System.Collections.Generic;

namespace Slipstream.Components.WinFormUI.Models
{
    public class EventInfoModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Type EventType { get; set; }

        public IList<EventPropertyInfoModel> Properties { get; set; }
    }
}
