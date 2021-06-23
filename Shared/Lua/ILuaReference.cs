#nullable enable

using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaReference : IDisposable
    {
        public string InstanceId { get; }
        public string LuaScriptInstanceId { get; }
    }
}