﻿#nullable enable

using Slipstream.Components.WebWidget.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.WebWidget.EventHandler
{
    internal class WebWidgetEventHandler : IEventHandler
    {
        public event EventHandler<WebWidgetCommandEvent>? OnWebWidgetCommandEvent;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                WebWidgetCommandEvent tev => OnEvent(OnWebWidgetCommandEvent, tev),
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