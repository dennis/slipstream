#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Audio.Lua
{
    public class AudioLuaLibrary : CommodityLuaLibrary<IAudioInstanceThread, AudioLuaReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static AudioLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitString("path")
                .PermitLong("output");
        }

        public AudioLuaLibrary(ILifetimeScope scope) : base(ConfigurationValidator, scope)
        {
        }

        protected override IAudioInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var outputDeviceIdx = cfg.ExtractOrDefault("output", -1);
            var path = cfg.ExtractOrDefault("path", "Audio");

            return scope.Resolve<IAudioInstanceThread>(
               new NamedParameter("instanceId", instanceId),
               new NamedParameter("output", outputDeviceIdx),
               new NamedParameter("path", path)
           );
        }
    }
}