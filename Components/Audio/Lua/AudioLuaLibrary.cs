#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Components.Audio.Lua
{
    public class AudioLuaLibrary : ILuaLibrary
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        private readonly object Lock = new object();
        private readonly Dictionary<string, IAudioInstanceThread> Instances = new Dictionary<string, IAudioInstanceThread>();
        private readonly ILifetimeScope LifetimeScope;
        public string Name => "api/audio";

        static AudioLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitString("path")
                .PermitLong("output");
        }

        public AudioLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var outputDeviceIdx = cfg.ExtractOrDefault("output", -1);
            var path = cfg.ExtractOrDefault("path", "Audio");

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                if (!Instances.TryGetValue(instanceId, out IAudioInstanceThread value))
                {
                    Debug.WriteLine($"Creating IAudioServiceThread for instanceId '{instanceId}'");

                    var service = LifetimeScope.Resolve<IAudioInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("output", outputDeviceIdx),
                        new NamedParameter("path", path)
                    );

                    Instances.Add(instanceId, service);
                    service.Start(); // TODO: Remove when no Instances
                }

                var instance = LifetimeScope.Resolve<AudioLuaReference>(
                    new NamedParameter("instanceId", instanceId)
                );

                return instance;
            }
        }

        public void Dispose()
        {
            foreach (var thread in Instances)
            {
                thread.Value.Dispose();
            }
            Instances.Clear();
        }
    }
}