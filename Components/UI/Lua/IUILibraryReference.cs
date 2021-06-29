﻿using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.UI.Lua
{
    public class IUILibraryReference : BaseLuaReference, ILuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IUIEventFactory EventFactory;
        private readonly ILogger Logger;
        private readonly string Prefix;

        public IUILibraryReference(
            string instanceId,
            string luaScriptInstanceId,
            string prefix,
            IEventBus eventBus,
            IUIEventFactory eventFactory,
            ILogger logger) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            Logger = logger;
            Prefix = prefix;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void print(string s)
        {
            Logger.Information($"{Prefix}: {s}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void create_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandCreateButton(Envelope, text));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void delete_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandDeleteButton(Envelope, text));
        }
    }
}