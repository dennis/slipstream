using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface ILuaContext : IDisposable
    {
        void HandleEvent(IEvent @event);
    }
}