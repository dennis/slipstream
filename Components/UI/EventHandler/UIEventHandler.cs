#nullable enable

using Slipstream.Components.UI.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.UI.EventHandler
{
    internal class UIEventHandler : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public UIEventHandler(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public event EventHandler<UIButtonTriggered>? OnUIButtonTriggered;

        public event EventHandler<UICommandCreateButton>? OnUICommandCreateButton;

        public event EventHandler<UICommandDeleteButton>? OnUICommandDeleteButton;

        public event EventHandler<UICommandWriteToConsole>? OnUICommandWriteToConsole;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                UICommandWriteToConsole tev => OnEvent(OnUICommandWriteToConsole, tev),
                UICommandCreateButton tev => OnEvent(OnUICommandCreateButton, tev),
                UICommandDeleteButton tev => OnEvent(OnUICommandDeleteButton, tev),
                UIButtonTriggered tev => OnEvent(OnUIButtonTriggered, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(Parent, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}