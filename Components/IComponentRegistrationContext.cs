using NLua;
using Slipstream.Backend;
using Slipstream.Components.Audio;
using Slipstream.Shared;
using System;

namespace Slipstream.Components
{
    internal interface IComponentRegistrationContext
    {
        public IEventBus EventBus { get; }

        public EventHandlerControllerBuilder EventHandlerControllerBuilder { get; }

        public void RegisterPlugin(string name, Func<IComponentPluginCreationContext, IPlugin> plugin);

        public void RegisterEventFactory<T>(Type type, T factory);

        public void RegisterEventHandler(Type type);

        void RegisterLuaGlue(ILuaGlueFactory luaGlueFactory);
    }
}