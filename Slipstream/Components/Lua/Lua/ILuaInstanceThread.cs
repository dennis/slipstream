#nullable enable

namespace Slipstream.Components.Lua.Lua
{
    public interface ILuaInstanceThread
    {
        void Start();

        void Join();

        void Stop();

        void Restart();
    }
}