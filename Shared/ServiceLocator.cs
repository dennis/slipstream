﻿using Slipstream.Backend.Services;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    public class ServiceLocator : IServiceLocator
    {
        internal readonly IDictionary<dynamic, dynamic> Factories = new Dictionary<dynamic, dynamic>();

        public ServiceLocator(IEventSerdeService eventSerdeService, IStateService stateService, ITxrxService txrxService)
        {
            Add<IEventSerdeService>(eventSerdeService);
            Add<IStateService>(stateService);
            Add<ITxrxService>(txrxService);
        }

        public T Get<T>()
        {
            return (T)Factories[typeof(T)];
        }

        public void Add<T>(T service)
        {
            Factories.Add(typeof(T), service);
        }
    }
}