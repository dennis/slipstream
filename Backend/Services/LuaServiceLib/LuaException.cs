using NLua.Exceptions;
using System;

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class LuaException : Exception
    {
        public LuaException(string error, LuaScriptException e) : base(error, e)
        {
        }
    }
}
