#nullable enable

using Autofac;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaReference : ILuaReference
    {
        private readonly ILifetimeScope LifetimeScope;
        private readonly DiscordLuaLibrary LuaLibrary;

        public string InstanceId { get; }

        public DiscordLuaReference(string instanceId, DiscordLuaLibrary luaLibrary, ILifetimeScope lifetimeScope)
        {
            InstanceId = instanceId;
            LifetimeScope = lifetimeScope;
            LuaLibrary = luaLibrary;
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
            LuaLibrary.ReferenceDropped(this);
        }
    }
}