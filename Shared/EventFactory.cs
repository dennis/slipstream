using Slipstream.Backend.Services;
using Slipstream.Shared.Factories;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class EventFactory : IEventFactory
    {
        internal readonly IDictionary<dynamic, dynamic> Factories = new Dictionary<dynamic, dynamic>();

        public EventFactory(IEventSerdeService eventSerdeService)
        {
            Factories.Add(typeof(IAudioEventFactory), new AudioEventFactory());
            Factories.Add(typeof(IFileMonitorEventFactory), new FileMonitorEventFactory());
            Factories.Add(typeof(IInternalEventFactory), new InternalEventFactory());
            Factories.Add(typeof(IIRacingEventFactory), new IRacingEventFactory());
            Factories.Add(typeof(ILuaEventFactory), new LuaEventFactory(eventSerdeService));
            Factories.Add(typeof(ITwitchEventFactory), new TwitchEventFactory());
            Factories.Add(typeof(IUIEventFactory), new UIEventFactory());
        }

        public T Get<T>()
        {
            return (T)Factories[typeof(T)];
        }
    }
}
