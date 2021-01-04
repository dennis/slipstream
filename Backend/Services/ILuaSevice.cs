#nullable enable

namespace Slipstream.Backend.Services
{
    public interface ILuaSevice
    {
        ILuaContext Parse(string filename, string logPrefix);
    }
}
