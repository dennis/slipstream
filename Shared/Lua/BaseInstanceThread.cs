#nullable enable

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Components.Internal.EventHandler;

using System;
using System.Threading;

namespace Slipstream.Shared.Lua
{
    public abstract class BaseInstanceThread : ILuaInstanceThread
    {
        protected readonly string InstanceId;
        protected readonly ILogger Logger;
        private Thread? ServiceThread;
        protected volatile bool Stopping = false;
        protected volatile bool AutoStart = false;
        protected IEventEnvelope InstanceEnvelope;
        protected IEventEnvelope BroadcastEnvelope;
        protected IEventBus EventBus;
        protected readonly string LuaLibraryName;
        protected IInternalEventFactory InternalEventFactory;

        protected BaseInstanceThread(
            string luaLLibraryName,
            string instanceId,
            ILogger logger,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IInternalEventFactory internalEventFactory
        )
        {
            InstanceId = instanceId;
            Logger = logger;
            InstanceEnvelope = new EventEnvelope(instanceId);
            BroadcastEnvelope = new EventEnvelope(instanceId);
            LuaLibraryName = luaLLibraryName;
            EventBus = eventBus;
            InternalEventFactory = internalEventFactory;

            var internalHandler = eventHandlerController.Get<Internal>();
            internalHandler.OnInternalCommandShutdown += (_, e) => Stopping = true;
            internalHandler.OnInternaDependencyAdded += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                    InstanceEnvelope = InstanceEnvelope.Add(e.InstanceId);
            };
            internalHandler.OnInternalDependencyRemoved += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                {
                    InstanceEnvelope = InstanceEnvelope.Remove(e.InstanceId);

                    if (InstanceEnvelope.Recipients == null || InstanceEnvelope.Recipients.Length == 0)
                        InactiveInstance();
                }
            };

            SetupThread();
        }

        protected virtual void InactiveInstance()
        {
            Logger.Information($"Instance '{InstanceId}' is inactive (not used by anyone)");
            Stopping = true;
        }

        private void SetupThread()
        {
            ServiceThread = new Thread(new ThreadStart(ThreadMain))
            {
                Name = GetType().Name + " " + InstanceId,
            };
        }

        private void ThreadMain()
        {
            Logger.Debug($"Starting {GetType().Name } {InstanceId}");

            try
            {
                Main();
            }
            catch (Exception e)
            {
                Logger.Error($"Exception in {GetType().Name} {InstanceId}: {e.Message}");
            }
            Logger.Debug($"Stopping {GetType().Name } {InstanceId}");

            if (AutoStart)
            {
                Logger.Debug($"Autostarting {GetType().Name } {InstanceId}");

                AutoStart = false;
                Stopping = false;
                ThreadMain();
                return;
            }

            EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceRemoved(BroadcastEnvelope, LuaLibraryName, InstanceId));
        }

        protected abstract void Main();

        public void Start()
        {
            if (ServiceThread?.ThreadState == ThreadState.Unstarted)
            {
                ServiceThread.Start();
                EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceAdded(BroadcastEnvelope, LuaLibraryName, InstanceId));
            }
            else if (ServiceThread?.ThreadState == ThreadState.Stopped)
            {
                Stopping = false;

                // We need to recreate it
                SetupThread();
                ServiceThread.Start();
                EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceAdded(BroadcastEnvelope, LuaLibraryName, InstanceId));
            }
        }

        public void Stop()
        {
            if (ServiceThread?.IsAlive == true)
                Stopping = true;
        }

        public void Restart()
        {
            if (ServiceThread?.IsAlive == true)
            {
                Stop();
                AutoStart = true;
            }
            else
            {
                Start();
            }
        }

        public void Join()
        {
            if (ServiceThread?.IsAlive == true)
                ServiceThread.Join();
        }

        public virtual void Dispose()
        {
            Stopping = true;
            if (ServiceThread?.IsAlive == true)
                ServiceThread?.Join();
            GC.SuppressFinalize(this);
        }
    }
}