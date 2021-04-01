#nullable enable

using System;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class EventHandlerControllerBuilder
    {
        private readonly List<Type> EventHandlers = new List<Type>();

        public void Add(Type t)
        {
            EventHandlers.Add(t);
        }

        public IEventHandlerController CreateEventHandlerController()
        {
            var controller = new EventHandlerController();

            foreach (var t in EventHandlers)
            {
                var instance = (IEventHandler)Activator.CreateInstance(t);

                controller.Add(instance);
            }

            return controller;
        }
    }
}