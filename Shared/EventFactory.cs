using Slipstream.Backend.Services;
using Slipstream.Shared.Factories;
using System;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class EventFactory : IEventFactory
    {
        private readonly IDictionary<dynamic, dynamic> Factories = new Dictionary<dynamic, dynamic>();

        public EventFactory(IEventSerdeService eventSerdeService)
        {
            Factories.Add(typeof(IInternalEventFactory), new InternalEventFactory());
            Factories.Add(typeof(ILuaEventFactory), new LuaEventFactory(eventSerdeService));
            Factories.Add(typeof(ITwitchEventFactory), new TwitchEventFactory());
            Factories.Add(typeof(IUIEventFactory), new UIEventFactory());
            Factories.Add(typeof(IPlaybackEventFactory), new PlaybackEventFactory());
        }

        public void Add<T>(Type factoryInterface, T factoryImplementation)
        {
            Factories.Add(factoryInterface, factoryImplementation);
        }

        public T Get<T>()
        {
            return (T)Factories[typeof(T)];
        }
    }
}