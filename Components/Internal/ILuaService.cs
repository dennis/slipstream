#nullable enable

using System;

namespace Slipstream.Components.Internal
{
    public interface ILuaService : IDisposable
    {
        ILuaContext Parse(string filename);

        void Loop();
    }
}