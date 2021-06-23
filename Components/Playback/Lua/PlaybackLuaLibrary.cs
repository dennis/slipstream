#nullable enable

using Autofac;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared;
using Slipstream.Components.Internal;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackLuaLibrary : BaseLuaLibrary<IPlaybackInstanceThread, IPlaybackLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static PlaybackLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id");
        }

        public PlaybackLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(ConfigurationValidator, scope, eventBus, eventFactory)
        {
        }

        protected override IPlaybackInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");

            return scope.Resolve<IPlaybackInstanceThread>(
                new NamedParameter("instanceId", instanceId)
            );
        }
    }
}