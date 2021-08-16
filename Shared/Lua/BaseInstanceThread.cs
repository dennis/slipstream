﻿#nullable enable

using Serilog;
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

        protected BaseInstanceThread(string instanceId, ILogger logger, IEventHandlerController eventHandlerController)
        {
            InstanceId = instanceId;
            Logger = logger;
            InstanceEnvelope = new EventEnvelope(instanceId);

            var internalHandler = eventHandlerController.Get<Internal>();
            internalHandler.OnInternalCommandShutdown += (_, e) => Stopping = true;
            internalHandler.OnInternaAddDependency += (_, e) => InstanceEnvelope = InstanceEnvelope.Add(e.InstanceId);
            internalHandler.OnInternalRemoveDependency += (_, e) => InstanceEnvelope = InstanceEnvelope.Remove(e.InstanceId);
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
            }
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
        }
    }
}
