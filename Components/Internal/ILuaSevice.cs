#nullable enable

namespace Slipstream.Components.Internal
{
    public interface ILuaSevice
    {
        ILuaContext Parse(string filename);

        void Loop();
    }
}