#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackLuaLibrary : ILuaLibrary
    {
        private static readonly DictionaryValidator ConfigurationValidator;
        private readonly object Lock = new object();
        private readonly Dictionary<string, IPlaybackInstanceThread> Instances = new Dictionary<string, IPlaybackInstanceThread>();
        private readonly ILifetimeScope LifetimeScope;
        public string Name => "api/playback";

        static PlaybackLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id");
        }

        public PlaybackLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                if (!Instances.TryGetValue(instanceId, out IPlaybackInstanceThread value))
                {
                    Debug.WriteLine($"Creating IAudioServiceThread for instanceId '{instanceId}'");

                    var instance = LifetimeScope.Resolve<IPlaybackInstanceThread>(
                        new NamedParameter("instanceId", instanceId)
                    );

                    Instances.Add(instanceId, instance);
                    instance.Start();
                }

                return LifetimeScope.Resolve<IPlaybackLuaReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }
    }
}