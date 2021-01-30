namespace Slipstream.Components.Internal
{
    public interface IStateService
    {
        void SetState(string key, string value, int lifetimeSeconds = 0);

        string GetState(string key);
    }
}