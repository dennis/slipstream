#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Components.UI.Lua
{
    public class UILuaLibrary : ILuaLibraryAutoRegistration
    {
        private readonly ILifetimeScope LifetimeScope;
        private static readonly DictionaryValidator ConfigurationValidator;

        public string Name => "api/ui";

        static UILuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitString("prefix");
        }

        public UILuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var prefix = cfg.ExtractOrDefault("prefix", "");

            return LifetimeScope.Resolve<IUILibraryReference>(
                new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("prefix", prefix)
            );
        }
    }
}