using NLua.Exceptions;
using System;

namespace Slipstream.Components.Internal.Services
{
    public class LuaException : Exception
    {
        public LuaException(string error, LuaScriptException e) : base(error, e)
        {
        }
    }
}