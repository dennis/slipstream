#nullable enable

using iRacingSDK;

namespace Slipstream.Components.IRacing.GameState
{
    internal interface IStateFactory
    {
        public IState? BuildState(DataSample ds);
    }
}