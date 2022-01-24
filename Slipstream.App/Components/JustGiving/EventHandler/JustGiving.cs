#nullable enable

using System;

using Slipstream.Shared;
using Slipstream.Components.JustGiving.Events;

namespace Slipstream.Components.JustGiving.EventHandler
{
    internal class JustGiving : IEventHandler
    {
        public event EventHandler<JustGivingDonation>? OnJustGivingDonation;

        public event EventHandler<JustGivingCommandSendDonations>? OnJustGivingCommandSendDonations;

        public event EventHandler<JustGivingInfo>? OnJustGivingInfo;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                JustGivingInfo tev => OnEvent(OnJustGivingInfo, tev),
                JustGivingDonation tev => OnEvent(OnJustGivingDonation, tev),
                JustGivingCommandSendDonations tev => OnEvent(OnJustGivingCommandSendDonations, tev),

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