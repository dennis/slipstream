using System;

namespace Slipstream.Shared
{
    public interface IInstanceIdTypeTracker
    {
        void Verify(string instanceId, string luaLibraryName);
    }
}