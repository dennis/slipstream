﻿#nullable enable

using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

using System;

namespace Slipstream.Components.Internal.EventHandler
{
    internal class Internal : IEventHandler
    {
        public event EventHandler<InternalCommandShutdown>? OnInternalCommandShutdown;

        public event EventHandler<InternalDependencyAdded>? OnInternaDependencyAdded;

        public event EventHandler<InternalDependencyRemoved>? OnInternalDependencyRemoved;

        public event EventHandler<InternalInstanceAdded>? OnInternalInstanceAdded;

        public event EventHandler<InternalInstanceRemoved>? OnInternalInstanceRemoved;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                InternalCommandShutdown tev => OnEvent(OnInternalCommandShutdown, tev),
                InternalDependencyAdded tev => OnEvent(OnInternaDependencyAdded, tev),
                InternalDependencyRemoved tev => OnEvent(OnInternalDependencyRemoved, tev),
                InternalInstanceAdded tev => OnEvent(OnInternalInstanceAdded, tev),
                InternalInstanceRemoved tev => OnEvent(OnInternalInstanceRemoved, tev),
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