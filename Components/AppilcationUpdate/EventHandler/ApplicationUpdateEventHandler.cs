#nullable enable

using Slipstream.Components.AppilcationUpdate.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.AppilcationUpdate.EventHandler
{
    internal class ApplicationUpdateEventHandler : IEventHandler
    {
        public event EventHandler<ApplicationUpdateLatestVersionChanged>? OnApplicationUpdateLatestVersionChanged;

        public event EventHandler<ApplicationUpdateCommandCheckLatestVersion>? OnApplicationUpdateCommandCheckLatestVersion;

        private readonly IEventHandlerController Parent;

        public ApplicationUpdateEventHandler(IEventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                ApplicationUpdateLatestVersionChanged tev => OnEvent(OnApplicationUpdateLatestVersionChanged, tev),
                ApplicationUpdateCommandCheckLatestVersion tev => OnEvent(OnApplicationUpdateCommandCheckLatestVersion, tev),
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