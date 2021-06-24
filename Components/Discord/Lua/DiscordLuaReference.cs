#nullable enable

using Autofac;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaReference : BaseLuaReference, ILuaReference
    {
        private readonly ILifetimeScope LifetimeScope;
        private readonly DiscordLuaLibrary LuaLibrary;

        public DiscordLuaReference(string instanceId, string luaScriptInstanceId, DiscordLuaLibrary luaLibrary, ILifetimeScope lifetimeScope) : base(instanceId, luaScriptInstanceId)
        {
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

        public override void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}