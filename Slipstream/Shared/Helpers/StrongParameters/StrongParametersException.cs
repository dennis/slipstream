using System;

#nullable enable

namespace Slipstream.Shared.Helpers.StrongParameters
{
    internal class StrongParametersException : Exception
    {
        public StrongParametersException() : base()
        {
        }

        public StrongParametersException(string message) : base(message)
        {
        }

        public StrongParametersException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}