#nullable enable

namespace Slipstream.Backend
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(string id, string name);
    }
}
