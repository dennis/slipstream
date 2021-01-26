#nullable enable

using Slipstream.Shared.EventHandlers;

namespace Slipstream.Shared
{
    public class EventHandlerControllerBuilder
    {
        public IEventHandlerController CreateEventHandlerController()
        {
            var controller = new EventHandlerController();

            controller.Add(new Internal(controller));
            controller.Add(new Lua(controller));
            controller.Add(new UI(controller));
            controller.Add(new Audio(controller));
            controller.Add(new IRacing(controller));
            controller.Add(new Twitch(controller));
            controller.Add(new FileMonitor(controller));
            controller.Add(new Playback(controller));

            return controller;
        }
    }
}