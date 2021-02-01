using NLua;
using System;

#nullable enable

namespace Slipstream.Components.Internal.LuaGlues
{
    internal partial class CoreLuaGlue
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