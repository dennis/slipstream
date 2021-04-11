using System;

namespace Slipstream.Components.FileMonitor.Lua
{
    public interface IFileMonitorInstanceThread : IDisposable
    {
        void Start();
    }
}