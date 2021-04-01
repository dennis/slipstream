using Serilog;
using Slipstream.Shared;
using System;

namespace Slipstream.Components
{
    internal interface IComponentRegistrationContext
    {
        public ILogger Logger { get; }

        public IEventBus EventBus { get; }

        public void RegisterPlugin(string name, Func<IComponentPluginCreationContext, IPlugin> plugin);

        public void RegisterEventFactory<T>(Type type, T factory);

        void RegisterLuaGlue(ILuaGlueFactory luaGlueFactory);
    }
}