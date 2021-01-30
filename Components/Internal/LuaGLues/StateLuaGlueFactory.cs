using Slipstream.Components.Internal.Services;

namespace Slipstream.Components.Internal.LuaGlues
{
    internal class StateLuaGlueFactory : ILuaGlueFactory
    {
        private readonly IStateService StateService;

        public StateLuaGlueFactory(IStateService stateService)
        {
            StateService = stateService;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            return new StateLuaGlue(StateService);
        }
    }
}