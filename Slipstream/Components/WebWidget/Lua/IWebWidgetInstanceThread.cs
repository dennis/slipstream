#nullable enable

using Slipstream.Shared.Lua;
using System;

namespace Slipstream.Components.WebWidget.Lua
{
    public interface IWebWidgetInstanceThread : ILuaInstanceThread, IDisposable
    {
    }
}