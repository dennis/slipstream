namespace Slipstream.Backend.Services
{
    public interface IStateService
    {
        void SetState(string key, string value);
        string GetState(string key);
    }
}
