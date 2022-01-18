#nullable enable

using Slipstream.Shared.Lua;

using System;

namespace Slipstream.Components.WebServer.Lua
{
    public interface IWebServerInstanceThread : ILuaInstanceThread, IDisposable
    {
    }
}