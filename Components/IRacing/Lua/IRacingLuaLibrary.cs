﻿#nullable enable

using Autofac;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingLuaLibrary : SingletonLuaLibrary<IIRacingInstanceThread, IIRacingReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static IRacingLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitBool("send_raw_state");
        }

        public IRacingLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(ConfigurationValidator, scope, eventBus, eventFactory)
        {
        }

        protected override IIRacingInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var publishRawState = cfg.ExtractOrDefault("send_raw_state", false);

            var subscription = EventBus.RegisterListener(instanceId);

            return LifetimeScope.Resolve<IIRacingInstanceThread>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("publishRawState", publishRawState),
                new TypedParameter(typeof(IEventBusSubscription), subscription)
            );
        }
    }
}