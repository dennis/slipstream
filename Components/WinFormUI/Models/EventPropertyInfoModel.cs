﻿using System;

namespace Slipstream.Components.WinFormUI.Models
{
    public class EventPropertyInfoModel
    {
        public string Name { get; set; } = string.Empty;
        public Type Type { get; set; }
        public bool IsComplex { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
