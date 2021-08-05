#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Audio.Lua
{
    public class AudioLuaLibrary : BaseLuaLibrary<IAudioInstanceThread, AudioLuaReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static AudioLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitString("path")
                .PermitLong("output");
        }

        public AudioLuaLibrary(ILifetimeScope scope, IEventBus eventBus) : base(ConfigurationValidator, scope, eventBus)
        {
        }

        protected override IAudioInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var outputDeviceIdx = cfg.ExtractOrDefault("output", -1);
            var path = cfg.ExtractOrDefault("path", "Audio");

            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IAudioInstanceThread>(
               new NamedParameter("luaLibraryName", Name),
               new NamedParameter("instanceId", instanceId),
               new NamedParameter("output", outputDeviceIdx),
               new NamedParameter("path", path),
               new TypedParameter(typeof(IEventBusSubscription), subscription)
           );
        }
    }
}