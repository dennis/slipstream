using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    public class ServiceLocator : IServiceLocator
    {
        internal readonly IDictionary<dynamic, dynamic> Services = new Dictionary<dynamic, dynamic>();

        public ServiceLocator(ILogger logger, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, IEventSerdeService eventSerdeService)
        {
            Services.Add(typeof(IEventSerdeService), eventSerdeService);
            Services.Add(typeof(ILuaService), new LuaService(logger, eventFactory, eventBus, stateService));
            Services.Add(typeof(IStateService), stateService);
            Services.Add(typeof(ITxrxService), new TxrxService(eventSerdeService));
        }

        public T Get<T>()
        {
            return (T)Services[typeof(T)];
        }
    }
}
