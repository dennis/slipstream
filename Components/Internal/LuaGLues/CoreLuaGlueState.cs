using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.Components.Internal.LuaGLues
{
    internal class CoreLuaGlueState : ILuaGlueState
    {
        private readonly IDictionary<string, DelayedExecution> DebounceDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly IDictionary<string, DelayedExecution> WaitDelayedFunctions = new Dictionary<string, DelayedExecution>();

        public CoreLuaGlueState()
        {
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }
    }
}