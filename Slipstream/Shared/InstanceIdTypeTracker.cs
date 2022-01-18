using System;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class InstanceIdTypeTracker : IInstanceIdTypeTracker
    {
        private readonly Dictionary<string, string> TrackedInstanceIds = new Dictionary<string, string>();

        public void Verify(string instanceId, string actualLuaLibraryName)
        {
            lock (TrackedInstanceIds)
            {
                if (TrackedInstanceIds.TryGetValue(instanceId, out string expectedLuaLibraryName))
                {
                    if (actualLuaLibraryName != expectedLuaLibraryName)
                    {
                        throw new InstanceIdWithUnexpectedTypeException(instanceId, actualLuaLibraryName, expectedLuaLibraryName);
                    }
                }
                else
                {
                    TrackedInstanceIds.Add(instanceId, actualLuaLibraryName);
                }
            }
        }
    }
}