namespace Slipstream.Shared
{
    public interface IEvent
    {
        string EventType { get; }
        bool ExcludeFromTxrx { get; }
    }
}
