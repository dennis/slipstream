using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Backend.Services
{
    public interface ILuaContext : IDisposable
    {
        void HandleEvent(IEvent @event);
    }
}