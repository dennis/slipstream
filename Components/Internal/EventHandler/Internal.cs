#nullable enable

using Slipstream.Components.Internal.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Internal.EventHandler
{
    internal class Internal : IEventHandler
    {
        public event EventHandler<InternalCommandPluginRegister>? OnInternalCommandPluginRegister;

        public event EventHandler<InternalCommandPluginStates>? OnInternalCommandPluginStates;

        public event EventHandler<InternalCommandPluginUnregister>? OnInternalCommandPluginUnregister;

        public event EventHandler<InternalPluginState>? OnInternalPluginState;

        public event EventHandler<InternalCommandShutdown>? OnInternalCommandShutdown;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                InternalCommandPluginRegister tev => OnEvent(OnInternalCommandPluginRegister, tev),
                InternalCommandPluginUnregister tev => OnEvent(OnInternalCommandPluginUnregister, tev),
                InternalPluginState tev => OnEvent(OnInternalPluginState, tev),
                InternalCommandPluginStates tev => OnEvent(OnInternalCommandPluginStates, tev),
                InternalCommandShutdown tev => OnEvent(OnInternalCommandShutdown, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}