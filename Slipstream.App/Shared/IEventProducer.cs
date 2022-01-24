namespace Slipstream.Shared
{
    public interface IEventProducer
    {
        void PublishEvent(IEvent e);
    }
}
