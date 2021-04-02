using Slipstream.Shared;
using System;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Components
{
    public interface IPlugin : IDisposable
    {
        public event EventHandler<BasePlugin>? OnStateChanged;

        public string Id { get; }
        public string Name { get; }
        public string DisplayName { get; }
        public IEventHandlerController EventHandlerController { get; }
        public bool Reconfigurable { get; }
        public bool FullThreadControl { get; }

        public void Run();

        public IEnumerable<ILuaGlue> CreateLuaGlues();
    }
}