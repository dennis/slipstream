#nullable enable

using Slipstream.Shared.Events.Internal;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class Internal : IEventHandler
    {
        private readonly EventHandler Parent;

        public Internal(EventHandler parent)
        {
            Parent = parent;
        }

        public delegate void OnInternalCommandPluginRegisterHandler(EventHandler source, EventHandlerArgs<InternalCommandPluginRegister> e);

        public delegate void OnInternalCommandPluginStatesHandler(EventHandler source, EventHandlerArgs<InternalCommandPluginStates> e);

        public delegate void OnInternalCommandPluginUnregisterHandler(EventHandler source, EventHandlerArgs<InternalCommandPluginUnregister> e);

        public delegate void OnInternalCommandReconfigureHandler(EventHandler source, EventHandlerArgs<InternalCommandReconfigure> e);

        public delegate void OnInternalPluginStateHandler(EventHandler source, EventHandlerArgs<InternalPluginState> e);

        public event OnInternalCommandPluginRegisterHandler? OnInternalCommandPluginRegister;

        public event OnInternalCommandPluginStatesHandler? OnInternalCommandPluginStates;

        public event OnInternalCommandPluginUnregisterHandler? OnInternalCommandPluginUnregister;

        public event OnInternalCommandReconfigureHandler? OnInternalCommandReconfigure;

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
                case InternalCommandReconfigure tev:
                    if (OnInternalCommandReconfigure != null)
                    {
                        OnInternalCommandReconfigure.Invoke(Parent, new EventHandlerArgs<InternalCommandReconfigure>(tev));
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