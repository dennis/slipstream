using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendWeatherInfo : IEvent
    {
        public string EventType => nameof(IRacingCommandSendWeatherInfo);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}