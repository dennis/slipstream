using Serilog;
using Slipstream.Backend;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
using EventHandler = Slipstream.Shared.EventHandler;

namespace Slipstream.Components
{
    public interface IComponentRegistration
    {
        internal void RegisterEvents(Type[] events);

        internal void RegisterPlugin(string pluginName, Func<Parameters, IPlugin> creator);

        internal void RegisterEventFactory<T>(Type factoryInterface, T implementation);

        internal void RegisterEventHandler(Type handlerInterface, IEventHandler implementation);
    }

    internal class RegistrationContext : IComponentRegistration
    {
        internal IEventFactory EventFactory { get; }
        internal EventHandler EventHandler { get; }
        internal IEventBus EventBus { get; }
        internal ILogger Logger { get; }
        private readonly List<Type> Events = new List<Type>();
        private readonly Dictionary<string, Func<Parameters, IPlugin>> Plugins = new Dictionary<string, Func<Parameters, IPlugin>>();

        internal RegistrationContext(IEventFactory eventFactory, EventHandler eventHandler, ILogger logger)
        {
            EventFactory = eventFactory;
            EventHandler = eventHandler;
            Logger = logger;
        }

        void IComponentRegistration.RegisterEvents(Type[] events)
        {
            Events.AddRange(events);
        }

        void IComponentRegistration.RegisterPlugin(string pluginName, Func<Parameters, IPlugin> creator)
        {
            Plugins.Add(pluginName, creator);
        }

        void IComponentRegistration.RegisterEventFactory<T>(Type factoryInterface, T implementation)
        {
            EventFactory.Add(factoryInterface, implementation);
        }

        void IComponentRegistration.RegisterEventHandler(Type handlerInterface, IEventHandler implementation)
        {
            EventHandler.Add(handlerInterface, implementation);
        }
    }
}