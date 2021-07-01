#nullable enable

using Slipstream.Components.WinFormUI.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.WinFormUI.EventHandler
{
    internal class WinFormUIEventHandler : IEventHandler
    {
        public event EventHandler<WinFormUIButtonTriggered>? OnWinFormUIButtonTriggered;

        public event EventHandler<WinFormUICommandCreateButton>? OnWinFormUICommandCreateButton;

        public event EventHandler<WinFormUICommandDeleteButton>? OnWinFormUICommandDeleteButton;

        public event EventHandler<WinFormUICommandWriteToConsole>? OnWinFormUICommandWriteToConsole;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                WinFormUICommandWriteToConsole tev => OnEvent(OnWinFormUICommandWriteToConsole, tev),
                WinFormUICommandCreateButton tev => OnEvent(OnWinFormUICommandCreateButton, tev),
                WinFormUICommandDeleteButton tev => OnEvent(OnWinFormUICommandDeleteButton, tev),
                WinFormUIButtonTriggered tev => OnEvent(OnWinFormUIButtonTriggered, tev),
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