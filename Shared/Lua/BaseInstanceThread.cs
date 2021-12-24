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
        private ulong _inactiveSince = 0;
        protected volatile bool Stopping = false;
        protected volatile bool AutoStart = false;
        protected IEventEnvelope InstanceEnvelope;
        protected IEventEnvelope BroadcastEnvelope;
        protected IEventBus EventBus;
        protected readonly string LuaLibraryName;
        protected IInternalEventFactory InternalEventFactory;
        public bool Stopped { get; internal set; }

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
            Logger = logger.ForContext(this.GetType()).ForContext("InstanceId", InstanceId);
            InstanceEnvelope = new EventEnvelope(instanceId);
            BroadcastEnvelope = new EventEnvelope(instanceId);
            LuaLibraryName = luaLLibraryName;
            EventBus = eventBus;
            InternalEventFactory = internalEventFactory;
            Stopped = false;

            Logger.Debug("Launching instance '{InstanceId}'", instanceId);

            var internalHandler = eventHandlerController.Get<Internal>();
            eventHandlerController.OnAllways += (_, e) =>
            {
                if (_inactiveSince > 0 && !Stopped && (e.Envelope.Uptime - _inactiveSince) > 10_000)
                {
                    InactiveInstanceDead();
                }
            };
            internalHandler.OnInternalCommandShutdown += (_, e) => Stopping = true;
            internalHandler.OnInternalDependencyAdded += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                    InstanceEnvelope = InstanceEnvelope.Add(e.InstanceId);

                if (_inactiveSince > 0)
                    ReactiveInstance(e.Envelope.Uptime);
            };
            internalHandler.OnInternalDependencyRemoved += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                {
                    InstanceEnvelope = InstanceEnvelope.Remove(e.InstanceId);

                    if (InstanceEnvelope.Recipients == null || InstanceEnvelope.Recipients.Length == 0)
                        InactiveInstance(e.Envelope.Uptime);
                }
            };

            SetupThread();
        }

        private void InactiveInstanceDead()
        {
            Logger.Debug("Instance '{InstanceId}' is died, stopping", InstanceId);
            Stop();
            _inactiveSince = 0;
        }

        private void ReactiveInstance(ulong _)
        {
            Logger.Debug("Instance '{InstanceId}' is reactivated", InstanceId);
            _inactiveSince = 0;
        }

        protected virtual void InactiveInstance(ulong uptime)
        {
            Logger.Information("Instance '{InstanceId}' is inactive (not used by anyone)", InstanceId);
            _inactiveSince = uptime;
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
            Logger.Debug("Starting {InstanceId}", InstanceId);

            try
            {
                Main();
            }
            catch (Exception e)
            {
                Logger.Error("Exception in {ExceptionName} {InstanceId}: {ExceptionMessage}", GetType().Name, InstanceId, e.Message, e);
            }
            Logger.Debug($"Stopping {GetType().Name } {InstanceId}");

            if (AutoStart)
            {
                Logger.Debug("Autostarting {InstanceId}", InstanceId);

                AutoStart = false;
                Stopping = false;
                ThreadMain();
                return;
            }

            EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceRemoved(BroadcastEnvelope, LuaLibraryName, InstanceId));
            Stopped = true;
        }

        protected abstract void Main();

        public void Start()
        {
            Logger.Debug($"Starting {InstanceId}", InstanceId);
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
            else
            {
                throw new Exception(ServiceThread?.ThreadState.ToString());
            }
        }

        public void Stop()
        {
            Logger.Debug($"Stopping {InstanceId}", InstanceId);
            if (ServiceThread?.IsAlive == true)
                Stopping = true;
        }

        public void Restart()
        {
            Logger.Debug($"Restarting {InstanceId}", InstanceId);
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
            Logger.Debug($"Joining {InstanceId}", InstanceId);
            if (ServiceThread?.IsAlive == true)
                ServiceThread.Join();
        }

        public virtual void Dispose()
        {
            Logger.Debug($"Disposing {InstanceId}", InstanceId);
            Stopping = true;
            if (ServiceThread?.IsAlive == true)
                ServiceThread?.Join();
            GC.SuppressFinalize(this);
        }
    }
}