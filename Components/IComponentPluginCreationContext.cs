using NLua;
using Slipstream.Shared.Helpers.StrongParameters;

namespace Slipstream.Components
{
    internal interface IComponentPluginCreationContext : IComponentPluginDependencies
    {
        string PluginId { get; }
        string PluginName { get; }
        Parameters PluginParameters { get; }
    }
}