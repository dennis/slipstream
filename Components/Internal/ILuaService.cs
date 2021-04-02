#nullable enable

namespace Slipstream.Components.Internal
{
    public interface ILuaService
    {
        ILuaContext Parse(string filename);

        void Loop();
    }
}