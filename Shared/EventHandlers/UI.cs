#nullable enable

using Slipstream.Shared.Events.UI;
using static Slipstream.Shared.EventHandlerController;

namespace Slipstream.Shared.EventHandlers
{
    internal class UI : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public UI(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnUIButtonTriggeredHandler(EventHandlerController source, EventHandlerArgs<UIButtonTriggered> e);

        public delegate void OnUICommandCreateButtonHandler(EventHandlerController source, EventHandlerArgs<UICommandCreateButton> e);

        public delegate void OnUICommandDeleteButtonHandler(EventHandlerController source, EventHandlerArgs<UICommandDeleteButton> e);

        public delegate void OnUICommandWriteToConsoleHandler(EventHandlerController source, EventHandlerArgs<UICommandWriteToConsole> e);

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