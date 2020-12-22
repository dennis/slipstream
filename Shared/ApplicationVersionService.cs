using System.Reflection;

namespace Slipstream.Shared
{
    class ApplicationVersionService : IApplicationVersionService
    {
        public string Version => Assembly
            .GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
    }
}
