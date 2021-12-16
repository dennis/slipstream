using System;

namespace Slipstream.Shared
{
    public class InstanceIdWithUnexpectedTypeException : Exception
    {
        public string InstanceId { get; }
        public string ActualLuaLibraryName { get; }
        public string ExpectedLuaLibraryName { get; }

        public override string Message => $"Id '{InstanceId}' with unexpected LuaLibrary '{ActualLuaLibraryName}', but was previously used with LuaLibrary '{ExpectedLuaLibraryName}'";

        public InstanceIdWithUnexpectedTypeException(string instanceId, string actualLuaLibraryName, string expectedLuaLibraryName)
        {
            InstanceId = instanceId;
            ActualLuaLibraryName = actualLuaLibraryName;
            ExpectedLuaLibraryName = expectedLuaLibraryName;
        }
    }
}