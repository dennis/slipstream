#nullable enable

namespace Slipstream.Backend.Services
{
    public interface ILuaService
    {
        ILuaContext Parse(string filename, string logPrefix);
    }
}
