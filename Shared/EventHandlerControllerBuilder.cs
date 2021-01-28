#nullable enable

using Slipstream.Shared.EventHandlers;
using System;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class EventHandlerControllerBuilder
    {
        private readonly List<Type> EventHandlers = new List<Type>();

        public EventHandlerControllerBuilder()
        {
            Add(typeof(Internal));
            Add(typeof(Lua));
            Add(typeof(UI));
            Add(typeof(IRacing));
            Add(typeof(Twitch));
            Add(typeof(FileMonitor));
            Add(typeof(Playback));
        }

        public void Add(Type t)
        {
            EventHandlers.Add(t);
        }

        public IEventHandlerController CreateEventHandlerController()
        {
            var controller = new EventHandlerController();

            foreach (var t in EventHandlers)
            {
                var instance = (IEventHandler)Activator.CreateInstance(t, new object[] { controller });

                controller.Add(instance);
            }

            return controller;
        }
    }
}