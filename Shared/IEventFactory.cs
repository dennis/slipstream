namespace Slipstream.Shared
{
    public interface IEventFactory
    {
        T Get<T>();
    }
}
