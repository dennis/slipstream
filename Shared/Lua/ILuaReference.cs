#nullable enable

using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaReference
    {
        public string InstanceId { get; }
        public string LuaScriptInstanceId { get; }
    }
}