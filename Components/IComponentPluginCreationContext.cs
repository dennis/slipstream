using NLua;
using Slipstream.Backend;
using Slipstream.Shared.Helpers.StrongParameters;
using System.Collections.Generic;

namespace Slipstream.Components
{
    internal interface IComponentPluginCreationContext : IComponentPluginDependencies
    {
        string PluginId { get; }
        string PluginName { get; }
        Parameters PluginParameters { get; }
        List<ILuaGlue> LuaGlues { get; }
        IPluginManager PluginManager { get; }
        IPluginFactory PluginFactory { get; }
    }
}