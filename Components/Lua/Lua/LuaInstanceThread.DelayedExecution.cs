#nullable enable

using NLua;
using System;

namespace Slipstream.Components.Lua.Lua
{
    public partial class LuaInstanceThread
    {
        private class DelayedExecution
        {
            public LuaFunction Function;
            public DateTime TriggersAt;

            public DelayedExecution(LuaFunction luaFunction, DateTime triggersAt)
            {
                Function = luaFunction;
                TriggersAt = triggersAt;
            }
        }
    }
}