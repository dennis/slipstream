using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    public class CapturingEventBus : IEventBus
    {
        private readonly IEventBus RealEventBus; // EventBus that actually sends events
        private readonly IEventBus CaptorEventBus; // EventBus that captures events instead of sending them
        private readonly List<IEvent> CapturedEventsList = new List<IEvent>();
        private IEventBus ActiveEventBus; // What we use currently

        class Captor : IEventBus
        {
            private readonly IEventBus EventBus;
            private readonly List<IEvent> CapturedEvents;

            public Captor(IEventBus eventBus, List<IEvent> capturingEventBus)
            {
                EventBus = eventBus;
                CapturedEvents = capturingEventBus;
            }

            public bool Enabled { get => EventBus.Enabled; set { EventBus.Enabled = value; } }

            public void PublishEvent(IEvent e)
            {
                CapturedEvents.Add(e);
            }

            public IEventBusSubscription RegisterListener(bool fromBeginning = false)
            {
                return EventBus.RegisterListener(fromBeginning);
            }

            public void UnregisterSubscription(IEventBusSubscription eventBusSubscription)
            {
                EventBus.UnregisterSubscription(eventBusSubscription);
            }
        }

        public bool Enabled { get => ActiveEventBus.Enabled; set { ActiveEventBus.Enabled = value; } }
        public IEvent[] CapturedEvents { get { return CapturedEventsList.ToArray(); } }

        public CapturingEventBus(IEventBus eventBus)
        {
            RealEventBus   = eventBus;
            CaptorEventBus = new Captor(eventBus, CapturedEventsList);
            ActiveEventBus = CaptorEventBus;
        }

        public void PublishEvent(IEvent e)
        {
            ActiveEventBus.PublishEvent(e);
        }

        public IEventBusSubscription RegisterListener(bool fromBeginning = false)
        {
            return ActiveEventBus.RegisterListener(fromBeginning);
        }

        public void UnregisterSubscription(IEventBusSubscription eventBusSubscription)
        {
            ActiveEventBus.UnregisterSubscription(eventBusSubscription);
        }

        public void StopCapturing()
        {
            CapturedEventsList.Clear();
            ActiveEventBus = RealEventBus;
        }
    }
}
