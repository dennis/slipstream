#nullable enable

using Slipstream.Shared.Events.UI;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class UI : IEventHandler
    {
        private readonly EventHandler Parent;

        public UI(EventHandler eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnUIButtonTriggeredHandler(EventHandler source, EventHandlerArgs<UIButtonTriggered> e);

        public delegate void OnUICommandCreateButtonHandler(EventHandler source, EventHandlerArgs<UICommandCreateButton> e);

        public delegate void OnUICommandDeleteButtonHandler(EventHandler source, EventHandlerArgs<UICommandDeleteButton> e);

        public delegate void OnUICommandWriteToConsoleHandler(EventHandler source, EventHandlerArgs<UICommandWriteToConsole> e);

        public event OnUIButtonTriggeredHandler? OnUIButtonTriggered;

        public event OnUICommandCreateButtonHandler? OnUICommandCreateButton;

        public event OnUICommandDeleteButtonHandler? OnUICommandDeleteButton;

        public event OnUICommandWriteToConsoleHandler? OnUICommandWriteToConsole;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case UICommandWriteToConsole tev:
                    if (OnUICommandWriteToConsole != null)
                    {
                        OnUICommandWriteToConsole.Invoke(Parent, new EventHandlerArgs<UICommandWriteToConsole>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case UICommandCreateButton tev:
                    if (OnUICommandCreateButton != null)
                    {
                        OnUICommandCreateButton.Invoke(Parent, new EventHandlerArgs<UICommandCreateButton>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case UICommandDeleteButton tev:
                    if (OnUICommandDeleteButton != null)
                    {
                        OnUICommandDeleteButton.Invoke(Parent, new EventHandlerArgs<UICommandDeleteButton>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case UIButtonTriggered tev:
                    if (OnUIButtonTriggered != null)
                    {
                        OnUIButtonTriggered.Invoke(Parent, new EventHandlerArgs<UIButtonTriggered>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}