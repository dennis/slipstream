using System;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    internal class ParameterException : Exception
    {
        public ParameterException() : base()
        {
        }

        public ParameterException(string message) : base(message)
        {
        }

        public ParameterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}