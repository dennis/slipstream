using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Backend.Services
{
    public interface ILuaContext : IDisposable
    {
        void Loop();
        void HandleEvent(IEvent @event);
    }
}
