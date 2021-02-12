#nullable enable

using iRacingSDK;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal interface IStateFactory
    {
        public IState? BuildState(DataSample ds);
    }
}