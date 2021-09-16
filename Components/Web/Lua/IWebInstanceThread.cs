#nullable enable

using Slipstream.Shared.Lua;

using System;

namespace Slipstream.Components.Web.Lua
{
    public interface IWebInstanceThread : ILuaInstanceThread, IDisposable
    {
    }
}