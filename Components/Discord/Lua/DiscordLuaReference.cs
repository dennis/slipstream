#nullable enable

using Autofac;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaReference : BaseLuaReference, ILuaReference
    {
        private readonly ILifetimeScope LifetimeScope;

        public DiscordLuaReference(string instanceId, string luaScriptInstanceId, ILifetimeScope lifetimeScope) : base(instanceId, luaScriptInstanceId)
        {
            LifetimeScope = lifetimeScope;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public IDiscordLuaChannelReference channel_id(long channelId)
        {
            return LifetimeScope.Resolve<IDiscordLuaChannelReference>(
                new NamedParameter("instanceId", InstanceId),
                new NamedParameter("channelId", channelId));
        }
    }
}