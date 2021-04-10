#nullable enable

using Autofac;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaReference : ILuaReference
    {
        private readonly string InstanceId;
        private readonly ILifetimeScope LifetimeScope;

        public DiscordLuaReference(string instanceId, ILifetimeScope lifetimeScope)
        {
            InstanceId = instanceId;
            LifetimeScope = lifetimeScope;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public IDiscordLuaChannelReference channel_id(long channelId)
        {
            return LifetimeScope.Resolve<IDiscordLuaChannelReference>(
                new NamedParameter("instanceId", InstanceId),
                new NamedParameter("channelId", channelId));
        }

        public void Dispose()
        {
        }
    }
}