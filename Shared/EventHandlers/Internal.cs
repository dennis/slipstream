#nullable enable

using Slipstream.Shared.Events.Internal;

namespace Slipstream.Shared.EventHandlers
{
    internal class Internal : IEventHandler
    {
        private readonly IEventHandlerController Parent;

        public Internal(IEventHandlerController parent)
        {
            Parent = parent;
        }

        public delegate void OnInternalCommandPluginRegisterHandler(IEventHandlerController source, EventHandlerArgs<InternalCommandPluginRegister> e);

        public delegate void OnInternalCommandPluginStatesHandler(IEventHandlerController source, EventHandlerArgs<InternalCommandPluginStates> e);

        public delegate void OnInternalCommandPluginUnregisterHandler(IEventHandlerController source, EventHandlerArgs<InternalCommandPluginUnregister> e);

        public delegate void OnInternalPluginStateHandler(IEventHandlerController source, EventHandlerArgs<InternalPluginState> e);

        public event OnInternalCommandPluginRegisterHandler? OnInternalCommandPluginRegister;

        public event OnInternalCommandPluginStatesHandler? OnInternalCommandPluginStates;

        public event OnInternalCommandPluginUnregisterHandler? OnInternalCommandPluginUnregister;

        public event OnInternalPluginStateHandler? OnInternalPluginState;

        public IEventHandler.HandledStatus HandleEvent(IEvent ev)
        {
            switch (ev)
            {
                // Internal

                case InternalCommandPluginRegister tev:
                    if (OnInternalCommandPluginRegister != null)
                    {
                        OnInternalCommandPluginRegister?.Invoke(Parent, new EventHandlerArgs<InternalCommandPluginRegister>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case InternalCommandPluginUnregister tev:
                    if (OnInternalCommandPluginUnregister != null)
                    {
                        OnInternalCommandPluginUnregister.Invoke(Parent, new EventHandlerArgs<InternalCommandPluginUnregister>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case InternalPluginState tev:
                    if (OnInternalPluginState != null)
                    {
                        OnInternalPluginState.Invoke(Parent, new EventHandlerArgs<InternalPluginState>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case InternalCommandPluginStates tev:
                    if (OnInternalCommandPluginStates != null)
                    {
                        OnInternalCommandPluginStates.Invoke(Parent, new EventHandlerArgs<InternalCommandPluginStates>(tev));
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