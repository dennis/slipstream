using System.Diagnostics;
using System.Reflection;

namespace Slipstream.Shared
{
    internal class ApplicationVersionService : IApplicationVersionService
    {
        public string Version { get; }

        public ApplicationVersionService()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.Version = versionInfo.FileVersion;
        }
    }
}
